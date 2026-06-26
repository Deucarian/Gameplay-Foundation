using System;
using System.Collections.Generic;

namespace Deucarian.GameplayFoundation
{
    /// <summary>Severity for a content validation issue.</summary>
    public enum ContentValidationSeverity
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }

    /// <summary>One validation issue from authored content, catalogs, or runtime-safe fixtures.</summary>
    public readonly struct ContentValidationIssue
    {
        public ContentValidationIssue(ContentValidationSeverity severity, string message, string path = null)
        {
            Severity = severity;
            Message = message ?? string.Empty;
            Path = path ?? string.Empty;
        }

        public ContentValidationSeverity Severity { get; }
        public string Message { get; }
        public string Path { get; }

        public static ContentValidationIssue Error(string message, string path = null)
        {
            return new ContentValidationIssue(ContentValidationSeverity.Error, message, path);
        }

        public static ContentValidationIssue Warning(string message, string path = null)
        {
            return new ContentValidationIssue(ContentValidationSeverity.Warning, message, path);
        }

        public static ContentValidationIssue Info(string message, string path = null)
        {
            return new ContentValidationIssue(ContentValidationSeverity.Info, message, path);
        }
    }

    /// <summary>Runtime-safe validation report for small content checks.</summary>
    public sealed class ContentValidationReport
    {
        private readonly List<ContentValidationIssue> _issues = new List<ContentValidationIssue>();

        public IReadOnlyList<ContentValidationIssue> Issues => _issues;
        public bool Succeeded => ErrorCount == 0;
        public bool IsValid => Succeeded;
        public int ErrorCount => Count(ContentValidationSeverity.Error);
        public int WarningCount => Count(ContentValidationSeverity.Warning);
        public int InfoCount => Count(ContentValidationSeverity.Info);

        public void AddIssue(ContentValidationIssue issue)
        {
            if (!string.IsNullOrWhiteSpace(issue.Message))
            {
                _issues.Add(issue);
            }
        }

        public void AddError(string message, string path = null)
        {
            AddIssue(ContentValidationIssue.Error(message, path));
        }

        public void AddWarning(string message, string path = null)
        {
            AddIssue(ContentValidationIssue.Warning(message, path));
        }

        public void AddInfo(string message, string path = null)
        {
            AddIssue(ContentValidationIssue.Info(message, path));
        }

        public string[] GetMessages(ContentValidationSeverity minimumSeverity = ContentValidationSeverity.Error)
        {
            var messages = new List<string>();
            for (int index = 0; index < _issues.Count; index++)
            {
                ContentValidationIssue issue = _issues[index];
                if (issue.Severity >= minimumSeverity)
                {
                    messages.Add(issue.Message);
                }
            }

            return messages.ToArray();
        }

        private int Count(ContentValidationSeverity severity)
        {
            int count = 0;
            for (int index = 0; index < _issues.Count; index++)
            {
                if (_issues[index].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    /// <summary>Known stable IDs used for reference checks.</summary>
    public sealed class ContentReferenceSet
    {
        public static readonly ContentReferenceSet Empty = new ContentReferenceSet(null);

        private readonly HashSet<string> _ids = new HashSet<string>(StringComparer.Ordinal);

        public ContentReferenceSet(IEnumerable<string> ids)
        {
            if (ids == null)
            {
                return;
            }

            foreach (string id in ids)
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    _ids.Add(id);
                }
            }
        }

        public int Count => _ids.Count;

        public bool Contains(string id)
        {
            return !string.IsNullOrWhiteSpace(id) && _ids.Contains(id);
        }

        public static ContentReferenceSet From<T>(IReadOnlyList<T> definitions, Func<T, string> resolveId)
        {
            if (definitions == null || definitions.Count == 0 || resolveId == null)
            {
                return Empty;
            }

            var ids = new string[definitions.Count];
            for (int index = 0; index < definitions.Count; index++)
            {
                T definition = definitions[index];
                ids[index] = ReferenceEquals(definition, null) ? string.Empty : resolveId(definition);
            }

            return new ContentReferenceSet(ids);
        }
    }

    /// <summary>Small reusable helpers for package- and template-owned content validation.</summary>
    public static class ContentValidation
    {
        public static ContentReferenceSet RequireUniqueIds<T>(
            IReadOnlyList<T> definitions,
            string label,
            Func<T, string> resolveId,
            ContentValidationReport report,
            bool requireAtLeastOne = false)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            var ids = new HashSet<string>(StringComparer.Ordinal);
            string resolvedLabel = ResolveLabel(label);
            if (definitions == null || definitions.Count == 0)
            {
                if (requireAtLeastOne)
                {
                    report.AddError($"At least one {resolvedLabel} definition is required.");
                }

                return new ContentReferenceSet(ids);
            }

            if (resolveId == null)
            {
                report.AddError($"Cannot validate {resolvedLabel} ids without an id resolver.");
                return new ContentReferenceSet(ids);
            }

            for (int index = 0; index < definitions.Count; index++)
            {
                T definition = definitions[index];
                if (ReferenceEquals(definition, null))
                {
                    report.AddError($"{resolvedLabel} definition at index {index} is null.");
                    continue;
                }

                string id = resolveId(definition);
                if (string.IsNullOrWhiteSpace(id))
                {
                    report.AddError($"{resolvedLabel} definition at index {index} is missing a stable id.");
                    continue;
                }

                if (!ids.Add(id))
                {
                    report.AddError($"Duplicate {resolvedLabel} id '{id}'.");
                }
            }

            return new ContentReferenceSet(ids);
        }

        public static bool RequireId(string id, string label, ContentValidationReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                return true;
            }

            report.AddError($"{ResolveLabel(label)} is missing a stable id.");
            return false;
        }

        public static void RequireReferences(
            IReadOnlyList<string> references,
            string label,
            ContentReferenceSet knownIds,
            ContentValidationReport report,
            bool requireAtLeastOne = false)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            string resolvedLabel = ResolveLabel(label);
            if (references == null || references.Count == 0)
            {
                if (requireAtLeastOne)
                {
                    report.AddError($"At least one {resolvedLabel} reference is required.");
                }

                return;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            ContentReferenceSet resolvedKnownIds = knownIds ?? ContentReferenceSet.Empty;
            for (int index = 0; index < references.Count; index++)
            {
                string id = references[index];
                if (string.IsNullOrWhiteSpace(id))
                {
                    report.AddError($"{resolvedLabel} reference at index {index} is empty.");
                    continue;
                }

                if (!seen.Add(id))
                {
                    report.AddError($"Duplicate {resolvedLabel} reference '{id}'.");
                }

                if (!resolvedKnownIds.Contains(id))
                {
                    report.AddError($"{resolvedLabel} reference '{id}' does not exist.");
                }
            }
        }

        public static bool RequireKnownReference(string id, string label, ContentReferenceSet knownIds, ContentValidationReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            string resolvedLabel = ResolveLabel(label);
            if (string.IsNullOrWhiteSpace(id))
            {
                report.AddError($"{resolvedLabel} reference is empty.");
                return false;
            }

            if ((knownIds ?? ContentReferenceSet.Empty).Contains(id))
            {
                return true;
            }

            report.AddError($"{resolvedLabel} reference '{id}' does not exist.");
            return false;
        }

        public static bool RequireGreaterThan(double value, double threshold, string label, ContentValidationReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (IsFinite(value) && value > threshold)
            {
                return true;
            }

            report.AddError($"{ResolveLabel(label)} must be greater than {threshold}.");
            return false;
        }

        public static bool RequireAtLeast(double value, double minimum, string label, ContentValidationReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (IsFinite(value) && value >= minimum)
            {
                return true;
            }

            report.AddError($"{ResolveLabel(label)} must be at least {minimum}.");
            return false;
        }

        private static bool IsFinite(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }

        private static string ResolveLabel(string label)
        {
            return string.IsNullOrWhiteSpace(label) ? "content" : label.Trim();
        }
    }
}

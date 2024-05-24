namespace Teapot.Web.Models;

public record AppInfo(string Sha, string ShortSha, string PreReleaseTag, string FullBuildMetaData, string BuildMetadata, string CommitDate);

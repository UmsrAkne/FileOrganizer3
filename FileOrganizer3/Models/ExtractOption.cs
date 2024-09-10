namespace FileOrganizer3.Models
{
    public enum ExtractOption
    {
        /// <summary>
        /// 全てのアイテムを対象にします。
        /// </summary>
        All,

        /// <summary>
        /// `IsIgnored` が `true` のアイテムを対象にします。
        /// </summary>
        Ignored,

        /// <summary>
        /// `IsMarked` が `true` のアイテムを対象にします。ただし、 `IsIgnored` が true のアイテムは対象外です。
        /// </summary>
        Marked,
    }
}
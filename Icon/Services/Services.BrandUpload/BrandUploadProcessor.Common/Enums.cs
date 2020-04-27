namespace BrandUploadProcessor.Common
{
    public static class Enums
    {
        public enum FileModeTypeEnum
        {
            CreateNew = 1,
            UpdateExisting = 2
        }

        public enum FileStatusEnum
        {
            New = 1,
            Processing = 2,
            Complete= 3,
            Error= 4
        }
    }
}

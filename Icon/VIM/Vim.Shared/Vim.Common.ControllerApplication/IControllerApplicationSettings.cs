namespace Vim.Common.ControllerApplication
{
    public interface IControllerApplicationSettings
    {
        int Instance { get; set; }
        int MaxNumberOfRowsToMark { get; set; }
        string ControllerName { get; set; }
    }
}

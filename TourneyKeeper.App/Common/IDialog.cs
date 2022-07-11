using System.Threading.Tasks;

namespace TourneyKeeper.Common
{
    public enum MessageResult
    {
        NONE = 0,
        OK = 1,
        CANCEL = 2,
        ABORT = 3,
        RETRY = 4,
        IGNORE = 5,
        YES = 6,
        NO = 7
    }

    public interface IDialog
    {
        Task<MessageResult> ShowDialog(string Title, string Message, bool SetCancelable = false, bool SetInverseBackgroundForced = false, MessageResult PositiveButton = MessageResult.OK, MessageResult NegativeButton = MessageResult.NONE, MessageResult NeutralButton = MessageResult.NONE);
    }
}

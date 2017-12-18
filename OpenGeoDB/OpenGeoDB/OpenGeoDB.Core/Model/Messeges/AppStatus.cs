using MvvmCross.Plugins.Messenger;

namespace OpenGeoDB.Core.Model.Messages
{
    public class AppStatus : MvxMessage
    {
        public StatusChange Status { get; }

        public AppStatus(object sender, StatusChange status)
            : base(sender)
        {
            Status = status;
        }

        public enum StatusChange
        {
            Start,
            Sleep,
            Resume,

            /// <summary>
            /// iOS exclusive 
            /// </summary>
            EnterBackground,
            /// <summary>
            /// iOS exclusive 
            /// </summary>
            EnterForeground
        }
    }
}
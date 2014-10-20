using EvolveQuest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvolveQuest.Shared.Interfaces
{
    public interface IMessageDialog
    {
        void SendToast(string toast);

        void SendMessage(string title, string message);

        void SendMessage(string title, string message, Action completed);

        void EnterTextMessage(string title, string message, Action<string> completed);

        void AskQuestions(string title, Question question, Action<int> completed);
    }
}

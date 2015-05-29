using System;
using Foundation;

namespace AppKineticsSaveEditService
{
    public interface IMainController
    {
        void ShowText(string text);

        void SetApplication(string application);

        void SetRequestId(string requestId);
    }
}


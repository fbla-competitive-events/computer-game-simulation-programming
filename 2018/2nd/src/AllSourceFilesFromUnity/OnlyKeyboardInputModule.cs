using System;
using UnityEngine.EventSystems;
class OnlyKeyBoardInputModule : StandaloneInputModule
{
	public override void Process()
	{
		bool usedEvent = SendUpdateEventToSelectedObject();

		if (eventSystem.sendNavigationEvents)
		{
			if (!usedEvent)
				usedEvent |= SendMoveEventToSelectedObject();

			if (!usedEvent)
				SendSubmitEventToSelectedObject();
		}
	}
}
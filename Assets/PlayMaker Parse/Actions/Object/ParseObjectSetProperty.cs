﻿// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Set a property of an Object. Leave ObjectId to none to target the current user. To save changes online, use the action ParseObjectSaveAsync")]
	public class ParseObjectSetProperty : FsmStateAction
	{

		public FsmString objectId;

		[RequiredField]
		public FsmString propertyKey;

		public FsmVar value;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;

		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			propertyKey = null;
			value = null;

			successEvent = null;
			errorEvent = null;
		}

		public override string ErrorCheck()
		{
			if (! PlayMakerParseProxy.IsPropertyKeyValid(propertyKey.Value))
			{
				return "Parse Property must only contain alphanumeric or underscore characters, and must begin with a letter";
			}
			return "";
		}

		public override void OnEnter()
		{
			ParseObject _object;

			if (objectId.IsNone)
			{
				_object = ParseUser.CurrentUser;
			}else{
				_object = PlayMakerParseProxy.GetParseObject(objectId.Value);
			}

			bool ok = PlayMakerParseProxy.SetParsePropertyFromFsmVar(_object,propertyKey.Value,this.Fsm,value);

			if (!ok )
			{
				Fsm.Event(errorEvent);
			}else{
				Fsm.Event(successEvent);
			}

			Finish();

		}
	}
}
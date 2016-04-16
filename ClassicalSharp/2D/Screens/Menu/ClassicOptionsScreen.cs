﻿// ClassicalSharp copyright 2014-2016 UnknownShadow200 | Licensed under MIT
using System;
using System.Drawing;
using ClassicalSharp.Singleplayer;

namespace ClassicalSharp.Gui {
	
	public class ClassicOptionsScreen : MenuOptionsScreen {
		
		public ClassicOptionsScreen( Game game ) : base( game ) {
		}
		
		public override void Init() {
			base.Init();
			INetworkProcessor network = game.Network;
			
			widgets = new Widget[] {
				// Column 1
				MakeClassicBool( -1, -150, "Music", OptionsKey.UseMusic,
				     OnWidgetClick, g => g.UseMusic,
				     (g, v) => { g.UseMusic = v; g.AudioPlayer.SetMusic( g.UseMusic ); }),
				
				MakeClassicBool( -1, -100, "Invert mouse", OptionsKey.InvertMouse,
				         OnWidgetClick, g => g.InvertMouse, (g, v) => g.InvertMouse = v ),
				
				MakeClassic2( -1, -50, "View distance", OnWidgetClick,
				     g => g.ViewDistance.ToString(),
				     (g, v) => g.SetViewDistance( Int32.Parse( v ), true ) ),
				
				!network.IsSinglePlayer ? null :
					MakeClassicBool( -1, 0, "Block physics", OptionsKey.SingleplayerPhysics, OnWidgetClick,
					     g => ((SinglePlayerServer)network).physics.Enabled,
					     (g, v) => ((SinglePlayerServer)network).physics.Enabled = v),
				
				// Column 2
				MakeClassicBool( 1, -150, "Sound", OptionsKey.UseSound,
				     OnWidgetClick, g => g.UseSound,
				     (g, v) => { g.UseSound = v; g.AudioPlayer.SetSound( g.UseSound ); }),
				
				MakeClassicBool( 1, -100, "Show FPS", OptionsKey.ShowFPS,
				     OnWidgetClick, g => g.ShowFPS, (g, v) => g.ShowFPS = v ),
				
				MakeClassicBool( 1, -50, "View bobbing", OptionsKey.ViewBobbing,
				     OnWidgetClick, g => g.ViewBobbing, (g, v) => g.ViewBobbing = v ),
				
				MakeClassic2( 1, 0, "FPS mode", OnWidgetClick,
				     g => g.FpsLimit.ToString(),
				     (g, v) => { object raw = Enum.Parse( typeof(FpsLimitMethod), v );
				     	g.SetFpsLimitMethod( (FpsLimitMethod)raw );
				     	Options.Set( OptionsKey.FpsLimit, v ); } ),
				
				!game.ClassicHacks ? null :
					MakeClassicBool( 0, 60, "Hacks enabled", OptionsKey.HacksEnabled,
					     OnWidgetClick, g => g.LocalPlayer.Hacks.Enabled,
				         (g, v) => { g.LocalPlayer.Hacks.Enabled = v;
					     	g.LocalPlayer.CheckHacksConsistency(); } ),
				
				MakeControlsWidget(),
				
				MakeBack( "Done", 25, titleFont, (g, w) => g.SetNewScreen( new PauseScreen( g ) ) ),
				null, null,
			};
			MakeValidators();
		}
		
		Widget MakeControlsWidget() {
			if( !game.ClassicHacks )
				return MakeClassic( 0, 110, "Controls", LeftOnly(
					(g, w) => g.SetNewScreen( new ClassicKeyBindingsScreen( g ) ) ), null, null );
			return MakeClassic( 0, 110, "Controls", LeftOnly(
				(g, w) => g.SetNewScreen( new ClassicHacksKeyBindingsScreen( g ) ) ), null, null );
		}
		
		void MakeValidators() {
			INetworkProcessor network = game.Network;
			validators = new MenuInputValidator[] {
				new BooleanValidator(),
				new BooleanValidator(),
				new IntegerValidator( 16, 4096 ),
				network.IsSinglePlayer ? new BooleanValidator() : null,			
				
				new BooleanValidator(),
				new BooleanValidator(),
				new BooleanValidator(),
				new EnumValidator( typeof(FpsLimitMethod) ),
				game.ClassicHacks ? new BooleanValidator() : null,
			};
		}
	}
}
﻿using System;
using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace Nez.Samples
{
	public class Ninja : Component, Nez.Mover.ITriggerListener, IUpdatable
	{
		enum Animations
		{
			WalkUp,
			WalkDown,
			WalkRight,
			WalkLeft
		}

		Sprite<Animations> _animation;
		Mover _mover;
		float _moveSpeed = 2.5f;


		public override void onAddedToEntity()
		{
			// load up our character texture atlas
			var texture = entity.scene.contentManager.Load<Texture2D>( "NinjaAdventure/characters/11" );
			var subtextures = Subtexture.subtexturesFromAtlas( texture, 16, 16 );

			_mover = entity.addComponent( new Mover() );
			_animation = entity.addComponent( new Sprite<Animations>( subtextures[0] ) );

			// extract the animations from the atlas
			_animation.addAnimation( Animations.WalkDown, new SpriteAnimation( new List<Subtexture>()
			{
				subtextures[0],
				subtextures[4],
				subtextures[8],
				subtextures[12]
			}) );

			_animation.addAnimation( Animations.WalkUp, new SpriteAnimation( new List<Subtexture>()
			{
				subtextures[1],
				subtextures[5],
				subtextures[9],
				subtextures[13]
			}) );

			_animation.addAnimation( Animations.WalkLeft, new SpriteAnimation( new List<Subtexture>()
			{
				subtextures[2],
				subtextures[6],
				subtextures[10],
				subtextures[14]
			}) );

			_animation.addAnimation( Animations.WalkRight, new SpriteAnimation( new List<Subtexture>()
			{
				subtextures[3],
				subtextures[7],
				subtextures[11],
				subtextures[15]
			}) );

			// add a circle collider
			entity.colliders.add( new CircleCollider() );
		}


		void IUpdatable.update()
		{
			var moveDir = Vector2.Zero;
			var animation = Animations.WalkDown;

			if( Input.isKeyDown( Keys.Left ) )
			{
				moveDir.X = -1f;
				animation = Animations.WalkLeft;
			}
			else if( Input.isKeyDown( Keys.Right ) )
			{
				moveDir.X = 1f;
				animation = Animations.WalkRight;
			}

			if( Input.isKeyDown( Keys.Up ) )
			{
				moveDir.Y = -1f;
				animation = Animations.WalkUp;
			}
			else if( Input.isKeyDown( Keys.Down ) )
			{
				moveDir.Y = 1f;
				animation = Animations.WalkDown;
			}


			if( moveDir != Vector2.Zero )
			{
				if( !_animation.isAnimationPlaying( animation ) )
					_animation.play( animation );
				
				var movement = moveDir * _moveSpeed;

				CollisionResult res;
				_mover.move( new Vector2( movement.X, 0 ), out res );
				_mover.move( new Vector2( 0, movement.Y ), out res );
			}
			else
			{
				_animation.stop();
			}
		}


		#region ITriggerListener implementation

		void Mover.ITriggerListener.onTriggerEnter( Collider other )
		{
			Debug.log( "triggerEnter: {0}", other.entity.name );
		}


		void Mover.ITriggerListener.onTriggerExit( Collider other )
		{
			Debug.log( "triggerExit: {0}", other.entity.name );
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace QuadTreeStudent
{
	class QuadTree
	{
		#region Constants
		// The maximum number of objects in a quad
		// before a subdivision occurs
		private const int MAX_OBJECTS_BEFORE_SUBDIVIDE = 3;
		#endregion

		#region Variables
		// The game objects held at this level of the tree
		private List<GameObject> _objects;

		// This quad's rectangle area
		private Rectangle _rect;

		// This quad's divisions
		private QuadTree[] _divisions;
		#endregion

		#region Properties
		/// <summary>
		/// The divisions of this quad
		/// </summary>
		public QuadTree[] Divisions { get { return _divisions; } }

		/// <summary>
		/// This quad's rectangle
		/// </summary>
		public Rectangle Rectangle { get { return _rect; } }

		/// <summary>
		/// The game objects inside this quad
		/// </summary>
		public List<GameObject> GameObjects { get { return _objects; } }
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new Quad Tree
		/// </summary>
		/// <param name="x">This quad's x position</param>
		/// <param name="y">This quad's y position</param>
		/// <param name="width">This quad's width</param>
		/// <param name="height">This quad's height</param>
		public QuadTree(int x, int y, int width, int height)
		{
			// Save the rectangle
			_rect = new Rectangle(x, y, width, height);

			// Create the object list
			_objects = new List<GameObject>();

			// No divisions yet
			_divisions = null;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds a game object to the quad.  If the quad has too many
		/// objects in it, and hasn't been divided already, it should
		/// be divided
		/// </summary>
		/// <param name="gameObj">The object to add</param>
		public void AddObject(GameObject gameObj)
		{
			// ACTIVITY: Complete this method
			if(_rect.Contains(gameObj.Rectangle))
			{
				//Console.WriteLine("This works!");
				if(_divisions == null)
				{
					if(_objects.Count >= MAX_OBJECTS_BEFORE_SUBDIVIDE){
						Divide();
					} else
					{
						
						_objects.Add(gameObj);
						return;
					}
				} 

				for(int i = 0; i < _divisions.Length; i++)
				{
					if(_divisions[i]._rect.Contains(gameObj.Rectangle))
					{
						_divisions[i].AddObject(gameObj);
						return;
					}
				}

				
				_objects.Add(gameObj);
			}
		}

		/// <summary>
		/// Divides this quad into 4 smaller quads.  Moves any game objects
		/// that are completely contained within the new smaller quads into
		/// those quads and removes them from this one.
		/// </summary>
		public void Divide()
		{
			// ACTIVITY: Complete this method
			_divisions = new QuadTree[4];

			int width = _rect.Width / 2;
			int height = _rect.Height / 2;
			
			_divisions[0] = new QuadTree(_rect.X,_rect.Y,width,height);
			_divisions[1] = new QuadTree(_rect.X + width,_rect.Y,width,height);
			_divisions[2] = new QuadTree(_rect.X,_rect.Y+height,width,height);
			_divisions[3] = new QuadTree(_rect.X + width,_rect.Y+height,width,height);

			
			List<GameObject> tempObjects = new List<GameObject>();
			tempObjects.AddRange(_objects);
			_objects.Clear();
			foreach(GameObject gameObj in tempObjects)
			{
				
				this.AddObject(gameObj);
			}
		}

		/// <summary>
		/// Recursively populates a list with all of the rectangles in this
		/// quad and any subdivision quads.  Use the "AddRange" method of
		/// the list class to add the elements from one list to another.
		/// </summary>
		/// <returns>A list of rectangles</returns>
		public List<Rectangle> GetAllRectangles()
		{
			List<Rectangle> rects = new List<Rectangle>();

			// ACTIVITY: Complete this method
			rects.Add(_rect);
			foreach(GameObject gameObj in _objects)
			{
				rects.Add(gameObj.Rectangle);
			}
			
			if(_divisions != null)
			{
				for(int i = 0; i < _divisions.Length; i++)
				{
					rects.AddRange(_divisions[i].GetAllRectangles());
				}	
			}
			return rects;
		}

		/// <summary>
		/// A possibly recursive method that returns the
		/// smallest quad that contains the specified rectangle
		/// </summary>
		/// <param name="rect">The rectangle to check</param>
		/// <returns>The smallest quad that contains the rectangle</returns>
		public QuadTree GetContainingQuad(Rectangle rect)
		{
			// ACTIVITY: Complete this method
			if(_rect.Contains(rect))
			{
				if(_divisions != null)
				{
					for(int i = 0; i < _divisions.Length; i++)
					{
						if(_divisions[i]._rect.Contains(rect))
						{
							return _divisions[i].GetContainingQuad(rect);
						}
					}
				}
				return this;
			}
			// Return null if this quad doesn't completely contain
			// the rectangle that was passed in
			return null;
		}
		#endregion
	}
}

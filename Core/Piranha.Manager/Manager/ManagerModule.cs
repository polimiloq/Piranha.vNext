﻿/*
 * Piranha CMS
 * Copyright (c) 2014, Håkan Edling, All rights reserved.
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library.
 */

using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using Piranha.Extend;

namespace Piranha.Manager
{
	/// <summary>
	/// The main entry point for the manager module.
	/// </summary>
	public class ManagerModule : IModule
	{
		#region Inner classes
		/// <summary>
		/// Class representing an embedded resource.
		/// </summary>
		internal class Resource
		{
			/// <summary>
			/// Gets/sets the resource name.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets/sets the content type.
			/// </summary>
			public string ContentType { get; set; }
		}
		#endregion

		#region Members
		/// <summary>
		/// Gets the internal collection of embedded resources.
		/// </summary>
		internal static IDictionary<string, Resource> Resources { get; private set; }

		/// <summary>
		/// Gets the last modification date of the manager module.
		/// </summary>
		internal static DateTime LastModified { get; private set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ManagerModule() {
			Resources = new Dictionary<string, Resource>();
			LastModified = new FileInfo(typeof(ManagerModule).Assembly.Location).LastWriteTime;
		}

		/// <summary>
		/// Initializes the module. This method should be used for
		/// ensuring runtime resources and registering hooks.
		/// </summary>
		public void Init() {
			// Alias
			Mapper.CreateMap<Piranha.Models.Alias, Models.Alias.ListItem>()
				.ForMember(a => a.Saved, o => o.Ignore())
				.ForMember(a => a.Created, o => o.MapFrom(m => m.Created.ToString("yyyy-MM-dd")))
				.ForMember(a => a.Updated, o => o.MapFrom(m => m.Updated.ToString("yyyy-MM-dd")));
			Mapper.CreateMap<Piranha.Models.Alias, Models.Alias.EditModel>();
			Mapper.CreateMap<Models.Alias.EditModel, Piranha.Models.Alias>()
				.ForMember(a => a.Id, o => o.Ignore())
				.ForMember(a => a.Created, o => o.Ignore())
				.ForMember(a => a.Updated, o => o.Ignore());

			// Author
			Mapper.CreateMap<Piranha.Models.Author, Models.Author.ListItem>()
				.ForMember(a => a.Saved, o => o.Ignore())
				.ForMember(a => a.GravatarUrl, o => o.Ignore())
				.ForMember(a => a.Created, o => o.MapFrom(m => m.Created.ToString("yyyy-MM-dd")))
				.ForMember(a => a.Updated, o => o.MapFrom(m => m.Updated.ToString("yyyy-MM-dd")));
			Mapper.CreateMap<Piranha.Models.Author, Models.Author.EditModel>()
				.ForMember(a => a.GravatarUrl, o => o.Ignore());
			Mapper.CreateMap<Models.Author.EditModel, Piranha.Models.Author>()
				.ForMember(a => a.Id, o => o.Ignore())
				.ForMember(a => a.Created, o => o.Ignore())
				.ForMember(a => a.Updated, o => o.Ignore());

			// Block
			Mapper.CreateMap<Piranha.Models.Block, Models.Block.ListItem>()
				.ForMember(b => b.Saved, o => o.Ignore())
				.ForMember(b => b.Created, o => o.MapFrom(m => m.Created.ToString("yyyy-MM-dd")))
				.ForMember(b => b.Updated, o => o.MapFrom(m => m.Updated.ToString("yyyy-MM-dd")));
			Mapper.CreateMap<Piranha.Models.Block, Models.Block.EditModel>();
			Mapper.CreateMap<Models.Block.EditModel, Piranha.Models.Block>()
				.ForMember(b => b.Id, o => o.Ignore())
				.ForMember(b => b.Created, o => o.Ignore())
				.ForMember(b => b.Updated, o => o.Ignore());

			// Category
			Mapper.CreateMap<Piranha.Models.Category, Models.Category.ListItem>()
				.ForMember(c => c.Saved, o => o.Ignore())
				.ForMember(c => c.Created, o => o.MapFrom(m => m.Created.ToString("yyyy-MM-dd")))
				.ForMember(c => c.Updated, o => o.MapFrom(m => m.Updated.ToString("yyyy-MM-dd")));
			Mapper.CreateMap<Piranha.Models.Category, Models.Category.EditModel>();
			Mapper.CreateMap<Models.Category.EditModel, Piranha.Models.Category>()
				.ForMember(b => b.Id, o => o.Ignore())
				.ForMember(b => b.Created, o => o.Ignore())
				.ForMember(b => b.Updated, o => o.Ignore());

			// Page type
			Mapper.CreateMap<Piranha.Models.PageType, Models.PageType.EditModel>()
				.ForMember(t => t.PropertyTypes, o => o.Ignore())
				.ForMember(t => t.RegionTypes, o => o.Ignore());
			Mapper.CreateMap<Models.PageType.EditModel, Piranha.Models.PageType>()
				.ForMember(t => t.Id, o => o.Ignore())
				.ForMember(t => t.Properties, o => o.Ignore())
				.ForMember(t => t.Regions, o => o.Ignore())
				.ForMember(t => t.Created, o => o.Ignore())
				.ForMember(t => t.Updated, o => o.Ignore());
			Mapper.CreateMap<Piranha.Models.PageTypeProperty, Models.PageType.EditModel.PagePart>();
			Mapper.CreateMap<Piranha.Models.PageTypeRegion, Models.PageType.EditModel.PagePart>();

			// Post
			Mapper.CreateMap<Piranha.Models.Post, Models.Post.EditModel>()
				.ForMember(p => p.Authors, o => o.Ignore())
				.ForMember(p => p.Categories, o => o.Ignore())
				.ForMember(p => p.Comments, o => o.Ignore())
				.ForMember(p => p.SelectedCategories, o => o.Ignore())
				.ForMember(p => p.Action, o => o.Ignore());
			Mapper.CreateMap<Models.Post.EditModel, Piranha.Models.Post>()
				.ForMember(p => p.Id, o => o.Ignore())
				.ForMember(p => p.Type, o => o.Ignore())
				.ForMember(p => p.Author, o => o.Ignore())
				.ForMember(p => p.Attachments, o => o.Ignore())
				.ForMember(p => p.Comments, o => o.Ignore())
				.ForMember(p => p.CommentCount, o => o.Ignore())
				.ForMember(p => p.Categories, o => o.Ignore())
				.ForMember(p => p.Created, o => o.Ignore())
				.ForMember(p => p.Updated, o => o.Ignore())
				.ForMember(p => p.Published, o => o.Ignore());

			// Post type
			Mapper.CreateMap<Piranha.Models.PostType, Models.PostType.EditModel>();
			Mapper.CreateMap<Models.PostType.EditModel, Piranha.Models.PostType>()
				.ForMember(t => t.Id, o => o.Ignore())
				.ForMember(t => t.IncludeInRss, o => o.Ignore())
				.ForMember(t => t.Created, o => o.Ignore())
				.ForMember(t => t.Updated, o => o.Ignore());

			Mapper.AssertConfigurationIsValid();

			// Register pre-compiled views
			AspNet.Hooks.RegisterPrecompiledViews += assemblies => {
				assemblies.Add(typeof(ManagerModule).Assembly);
			};

			// Scan precompiled resources
			foreach (var name in typeof(ManagerModule).Assembly.GetManifestResourceNames()) {
				Resources.Add(name.Replace("Piranha.Areas.Manager.Assets.", "").ToLower(), new Resource() {
					Name = name, ContentType = GetContentType(name)
				}) ;
			}
		}

		#region Private methods
		/// <summary>
		/// Gets the content type from the resource name.
		/// </summary>
		/// <param name="name">The resource name</param>
		/// <returns>The content type</returns>
		private string GetContentType(string name) {
			if (name.EndsWith(".js")) {
				return "text/javascript" ;
			} else if (name.EndsWith(".css")) {
				return "text/css" ;
			} else if (name.EndsWith(".png")) {
				return "image/png" ;
			} else if (name.EndsWith(".jpg")) {
				return "image/jpg" ;
			} else if (name.EndsWith(".gif")) {
				return "image/gif" ;
			} else if (name.EndsWith(".ico")) {
				return "image/ico" ;
			} else if (name.EndsWith(".eot")) {
				return "application/vnd.ms-fontobject" ;
			} else if (name.EndsWith(".ttf")) {
				return "application/octet-stream" ;
			} else if (name.EndsWith(".svg")) {
				return "image/svg+xml" ;
			} else if (name.EndsWith(".woff")) {
				return "application/x-woff" ;
			}
			return "application/unknown" ;
		}
		#endregion
	}
}
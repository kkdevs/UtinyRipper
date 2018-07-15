
using System;
using System.Collections.Generic;
using System.IO;
using UtinyRipper.Classes;
using UtinyRipper;

public class Program
{
	public static void print(string s)
	{
		Console.WriteLine(s);
	}
	public static void Main(string[] args)
	{
		HashSet<uint> paths = new HashSet<uint>();
		Dictionary<uint, string> bones = new Dictionary<uint, string>();
		foreach (var dir in args)
		{
			foreach (var fn in Directory.GetFiles(dir, "*.unity3d", SearchOption.TopDirectoryOnly))
			{
				var coll = new FileCollection();
				coll.Load(fn);
				foreach (var asset in coll.FetchAssets())
				{
					var clip = asset as AnimationClip;
					var avatar = asset as Avatar;
					if (clip != null)
					{
						foreach (var binding in clip.ClipBindingConstant.GenericBindings)
						{
							paths.Add(binding.Path);
						}
					}
					if (avatar != null)
					{
						foreach (var kv in avatar.m_TOS)
							bones[kv.Key] = kv.Value;
					}
				}
			}
		}
		foreach (var pathid in paths)
		{
			if (bones.TryGetValue(pathid, out string path))
			{
				print($"{pathid} {path}");
			}
			else
			{
				print($"Unresolved {pathid}");
			}
		}
	}
}
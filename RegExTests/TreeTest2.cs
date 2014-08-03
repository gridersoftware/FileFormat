using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using GSLib.Collections.Trees;

namespace RegExTests
{
	public class TreeTest
	{
		public Int32 Rootcount { get; private set; }
		public TreeNode<Node>[] Nodes { get; private set; }

		public struct Node
		{
			public Int32 Value { get; private set; }

			public Node(Int32 value)
			{
				this = new Node();
				Value = value;
			}

			public Node(BinaryReader reader)
			{
				this = new Node();
				try
				{
					Value = reader.ReadInt32();
				}
				catch
				{
					throw;
				}
			}

			public void Save(BinaryWriter writer)
			{
				try
				{
					writer.Write(Value);
				}
				catch
				{
					throw;
				}
			}
		}
		public TreeTest(Int32 rootcount, TreeNode<Node>[] nodes)
		{
			Rootcount = rootcount;
			if (nodes.Length != Rootcount) throw new ArgumentOutOfRangeException();
			Nodes = nodes;
		}

		public TreeTest(string filename)
		{
			try
			{
				if (File.Exists(filename))
				{
					using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
					{
						Rootcount = reader.ReadInt32();
						Nodes = new TreeNode<Node>[Rootcount];

						for (int i = 0; i < Rootcount; i++)
						{
							ReadTreeNode(Nodes[i], reader);
						}

						reader.Close();
					}
				}
				else
				{
					throw new FileNotFoundException();
				}
			}
			catch
			{
				throw;
			}
		}

		public void SaveFile(string filename, FileMode filemode)
		{
			try
			{
				using (BinaryWriter writer = new BinaryWriter(File.Open(filename, filemode)))
				{
					writer.Write(Rootcount);
					for (int i = 0; i < Rootcount; i++)
					{
						SaveTreeNode(Nodes[i], writer);
					}

					writer.Close();
				}
			}
			catch
			{
				throw;
			}
		}

		private void SaveTreeNode(TreeNode<Node> _node, BinaryWriter writer)
		{
			writer.Write(_node.Count);
			_node.Value.Save(writer);

			TreeNode<Node>[] children = _node.ToArray();

			foreach (TreeNode<Node> child in children)
			{
				SaveTreeNode(child, writer);
			}
		}

		private void ReadTreeNode(TreeNode<Node> _node, BinaryReader reader)
		{
			int count = reader.ReadInt32();
			Node[] nodes = new Node[count];
			_node = new TreeNode<Node>(new Node(reader));

			for (int i = 0; i < count; i++)
			{
				_node.Add(nodes[i]);
				ReadTreeNode(_node[i], reader);
			}
		}
	}
}

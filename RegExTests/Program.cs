using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using GSLib.Collections.Trees;

namespace RegExTests
{
    class Program
    {
        static void Main(string[] args)
        {
            TreeNode<TreeTest.Node> root = new TreeNode<TreeTest.Node>(new TreeTest.Node(5));
            root.Add(new TreeTest.Node(12));
            root[0].Add(new TreeTest.Node(6));
            root[0].Add(new TreeTest.Node(9));
            root.Add(new TreeTest.Node(17));
            root[1].Add(new TreeTest.Node(23));
            root[1].Add(new TreeTest.Node(100));
            root[1].Add(new TreeTest.Node(7));
            root.Add(new TreeTest.Node(35));
            root[2].Add(new TreeTest.Node(18));
            root[2][0].Add(new TreeTest.Node(50));
            root[2][0].Add(new TreeTest.Node(6));

            TreeTest file = new TreeTest(1, new TreeNode<TreeTest.Node>[] { root });
            file.SaveFile("testtree", FileMode.Create);

            TreeTest file2 = new TreeTest("testtree");
            TreeNode<TreeTest.Node> root2 = file.Nodes[0];

            Console.ReadLine();
        }
    }
}

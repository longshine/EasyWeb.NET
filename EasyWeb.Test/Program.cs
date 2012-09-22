﻿using System;
using System.Reflection;

namespace LX.EasyWeb.Test
{
    class Program
    {
        static void Main()
        {
            RunTests();
            Console.WriteLine("(end of tests; press any key)");
            Console.ReadKey();
        }

        static void RunTests()
        {
            var tester = new Tests();
            int fail = 0;
            MethodInfo[] methods = typeof(Tests).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var activeTests = Array.FindAll(methods, delegate(MethodInfo m) { return Attribute.IsDefined(m, typeof(ActiveTestAttribute)); });
            if (activeTests.Length != 0) methods = activeTests;
            foreach (var method in methods)
            {
                Console.Write("Running " + method.Name);
                try
                {
                    method.Invoke(tester, null);
                    Console.WriteLine(" - OK!");
                }
                catch (Exception ex)
                {
                    fail++;
                    if (ex.InnerException == null)
                        Console.WriteLine(" - " + ex.Message);
                    else if (ex.InnerException.InnerException == null)
                        Console.WriteLine(" - " + ex.InnerException.Message);
                    else
                        Console.WriteLine(" - " + ex.InnerException.InnerException.Message);
                }
            }
            Console.WriteLine();
            if (fail == 0)
            {
                Console.WriteLine("(all tests successful)");
            }
            else
            {
                Console.WriteLine("#### FAILED: {0} / {1}", fail, methods.Length);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    sealed class ActiveTestAttribute : Attribute { }
}

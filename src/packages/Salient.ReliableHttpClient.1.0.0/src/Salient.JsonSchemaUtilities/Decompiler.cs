using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Ast;
using Mono.Cecil;
namespace Salient.JsonSchemaUtilities
{
    public class Decompiler
    {
        public void Decompile()
        {
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly("Salient.JsonSchemaUtilities.dll");
            DecompilerSettings settings = new DecompilerSettings();
            settings.FullyQualifyAmbiguousTypeNames = false;
            AstBuilder decompiler = new AstBuilder(new DecompilerContext(assembly.MainModule) { Settings = settings });
            decompiler.AddAssembly(assembly);
            //new Helpers.RemoveCompilerAttribute().Run(decompiler.CompilationUnit);
            StringWriter output = new StringWriter();
            decompiler.GenerateCode(new PlainTextOutput(output));
            var code = output.ToString();
        }
    }
}

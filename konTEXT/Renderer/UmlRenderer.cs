using konTEXT.Models;
using System.Text;
using konTEXT.Tools;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Debugger.Interop;

namespace konTEXT.Renderer
{
    public class UmlRenderer
    {
        private const string BackgroundColor = "#GreenYellow/LightGoldenRodYellow";

        public static string Render(ClassModel classModel)
        {
            var sb = new StringBuilder();

            sb.AppendLine("@startuml");//desc start
            sb.AppendFormat("package {0} {1}", classModel.NamespaceDeclarationSyntax.Name, BackgroundColor).Append("{").AppendLine();//namespace start
            AddBaseObjects(classModel, sb);
            sb.AppendFormat("class {0} ", classModel.ClassDeclarationSyntax.Identifier.ValueText).Append("{").AppendLine();//class start
            AddConstructors(classModel, sb);
            AddEvents(classModel, sb);
            AddFields(classModel, sb);
            AddProps(classModel, sb);
            AddMethods(classModel, sb);
            AddEnums(classModel, sb);

            sb.AppendLine("}");//class close
            sb.AppendLine("}");//namespace close
            
            sb.AppendLine("@enduml");//desc close

            return sb.ToString();
        }

        private static void AddBaseObjects(ClassModel classModel, StringBuilder sb)
        {
            if (classModel.ClassDeclarationSyntax.BaseList != null)
            {
                foreach (var baseType in classModel.ClassDeclarationSyntax.BaseList.Types)
                {
                    if (baseType.Type.ToString().StartsWith("I"))
                    {
                        sb.AppendFormat("class {0} implements {1}", classModel.ClassDeclarationSyntax.Identifier.ValueText, (baseType.Type.ToString())).AppendLine();
                    }
                    else
                    {
                        sb.AppendFormat("class {0} extends {1}", classModel.ClassDeclarationSyntax.Identifier.ValueText, (baseType.Type.ToString())).AppendLine();
                    }
                }
            }
        }

        private static void AddConstructors(ClassModel classModel, StringBuilder sb)
        {
            using (var e = classModel.ConstructorDeclarationSyntax.GetEnumerator())
            {
                var allowCaption = true;
                
                while (e.MoveNext().Equals(true))
                {
                    if (e.Current != null)
                    {
                        if (allowCaption)
                        {
                            sb.AppendLine(".. Constructors ..");
                            allowCaption = false;
                        }

                        sb.AppendFormat("{0}{1}", e.Current.Modifiers.GetUmlModifiers(), e.Current.Identifier)
                            .Append("()").AppendLine();
                    }
                        
                }
            }
        }
        
        private static void AddEvents(ClassModel classModel, StringBuilder sb)
        {
            //Todo add events
        }

        private static void AddFields(ClassModel classModel, StringBuilder sb)
        {
            using (var e = classModel.FieldDeclarationSyntax.GetEnumerator())
            {
                var allowCaption = true;

                while (e.MoveNext().Equals(true))
                {
                    if (e.Current != null)
                    {
                        if (allowCaption)
                        {
                            sb.AppendLine(".. Fields ..");
                            allowCaption = false;
                        }

                        sb.AppendFormat("{0}{1}", e.Current.Modifiers.GetUmlModifiers(), e.Current.Declaration)
                            .AppendLine();
                    }

                }
            }
        }

        private static void AddProps(ClassModel classModel, StringBuilder sb)
        {
            using (var e = classModel.PropertyDeclarationSyntax.GetEnumerator())
            {
                var allowCaption = true;

                while (e.MoveNext().Equals(true))
                {
                    if (e.Current != null)
                    {
                        if (allowCaption)
                        {
                            sb.AppendLine(".. Properties ..");
                            allowCaption = false;
                        }

                        sb.AppendFormat("{0}{1}", e.Current.Modifiers.GetUmlModifiers(), e.Current.Identifier)
                            .AppendLine();
                    }

                }
            }
        }

        private static void AddMethods(ClassModel classModel, StringBuilder sb)
        {
            using (var e = classModel.MethodDeclarationSyntax.GetEnumerator())
            {
                var allowCaption = true;

                while (e.MoveNext().Equals(true))
                {
                    if (e.Current != null)
                    {
                        if (allowCaption)
                        {
                            sb.AppendLine(".. Methods ..");
                            allowCaption = false;
                        }

                        sb.AppendFormat("{0}{1} {2}", e.Current.Modifiers.GetUmlModifiers(), e.Current.ReturnType, e.Current.Identifier)
                            .Append("()").AppendLine();
                    }

                }
            }
        }

        private static void AddEnums(ClassModel classModel, StringBuilder sb)
        {
            //Todo add enums
        }
    }
}

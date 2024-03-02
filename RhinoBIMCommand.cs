using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Render.ChangeQueue;
using Rhino.Render.DataSources;
using System;
using System.Collections.Generic;

namespace RhinoBIM
{
    public class GridX : Command
    {
        public GridX()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static GridX Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "GridX";

        
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            // ---
            RhinoApp.WriteLine("The {0} command will create a the grides in x direction", EnglishName);

            // Solicitar al usuario el nombre del nuevo layer
            GetString getLayerName = new GetString();
            getLayerName.SetCommandPrompt("Enter the name of the new layer: ");
            getLayerName.AcceptNothing(false);
            getLayerName.Get();

            if (getLayerName.CommandResult() != Result.Success)
                return getLayerName.CommandResult();

            string layerName = getLayerName.StringResult();

            // Verificar si el layer existe
            if (doc.Layers.FindName(layerName)!= null)
            {
                // El layer ya existe
                RhinoApp.WriteLine($"El layer '{layerName}' ya existe.");
                return Result.Cancel;
            }

            // Crear el nuevo layer
            Layer newLayer = new Layer();
            newLayer.Name = layerName;

            // Añadir el nuevo layer al documento
            int newIndex = doc.Layers.Add(newLayer);
            if (newIndex < 0)
            {
                RhinoApp.WriteLine("Error creating the new layer.");
                return Result.Failure;
            }

            RhinoApp.WriteLine($"New layer '{layerName}' created successfully.");
      

            using (GetNumber lenght = new GetNumber())
            using (GetNumber spaces = new GetNumber())
            using (GetNumber number_of_lines = new GetNumber())

            {
                lenght.SetCommandPrompt("Input length of the lines: ");
                lenght.AcceptNumber(false, false);
                lenght.Get();

                spaces.SetCommandPrompt("Input spacing between lines: ");
                spaces.AcceptNumber(false, false);
                spaces.Get();

                number_of_lines.SetCommandPrompt("Input number of lines: "); 
                number_of_lines.AcceptNumber(false, false);
                number_of_lines.Get();

                if (lenght.CommandResult() == Result.Success && spaces.CommandResult() == Result.Success && number_of_lines.CommandResult() == Result.Success)
                {
                    double lineLength = lenght.Number();
                    double spacing = spaces.Number();
                    int numberOfLines = (int)number_of_lines.Number();

                    Line Linea1 = new Line(0, 0, 0, lineLength, 0, 0);
                    
               
                    doc.Objects.AddLine(Linea1);

                    for (int i = 1; i < numberOfLines; i++) 
                    {
                        double newY = i * spacing; 
                        Line newLine = new Line(0, newY, 0, lineLength, newY, 0);
                        doc.Objects.AddLine(newLine);
                    }

                    doc.Views.Redraw();


                }


            }



                return Result.Success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Rhino.Geometry;
using System.Windows.Forms;

namespace GHDisplayOptions
{
    public class GHDisplayMenu : GH_AssemblyPriority
    {
        private GH_Canvas Owner;


        public GHDisplayMenu() : base() { }

        public override GH_LoadingInstruction PriorityLoad()
        {
            Task task = new Task(OnStartup);
            task.Start();

            return GH_LoadingInstruction.Proceed;
        }

        private void OnStartup()
        {

            Instances.CanvasCreated += OnCanvasCreated;

            GH_DocumentEditor editor = null;

            while (editor == null)
            {
                editor = Grasshopper.Instances.DocumentEditor;
                Thread.Sleep(200);
            }

            Populate(editor.MainMenuStrip);

        }

        private void Populate(MenuStrip mms)
        {
            var tl = "DisplayControl";

            ToolStripMenuItem menu = new ToolStripMenuItem(tl);

            mms.Items.Add(menu);

            PopulateSub(mms, menu);
        }

        

        private void PopulateSub(MenuStrip mms, ToolStripMenuItem menu)
        {
            MethodInvoker method1 = delegate
            {
                var selectedText = new ToolStripMenuItem("Change Selected Display to Text");
                selectedText.ShortcutKeys = Keys.Control | Keys.T;
                selectedText.Click += OnSelectText_Click;
                menu.DropDownItems.Add(selectedText);
            };


            MethodInvoker method2 = delegate
            {
                var selectedIcon = new ToolStripMenuItem("Change Selected Display to Icon");
                selectedIcon.ShortcutKeys = Keys.Control | Keys.R;
                selectedIcon.Click += OnSelectIcon_Click;
                menu.DropDownItems.Add(selectedIcon);
            };

            MethodInvoker method3 = delegate
            {
                var selectedApp = new ToolStripMenuItem("Change Selected Display to Application Setting");
                
                selectedApp.Click += OnSelectApp_Click;
                menu.DropDownItems.Add(selectedApp);
            };

            mms.BeginInvoke(method1);
            mms.BeginInvoke(method2);
            mms.BeginInvoke(method3);
        }

        private void OnSelectText_Click(object sender, EventArgs e)
        {
            if (Owner != null)
            {

                if (Owner.IsDocument)
                {
                    GH_Document doc = Owner.Document;
                    foreach(IGH_DocumentObject docObject in doc.SelectedObjects())
                    {
                        if (docObject.IconDisplayMode == GH_IconDisplayMode.application)
                        {
                            docObject.IconDisplayMode = GH_IconDisplayMode.name;
                        }
                        else if(docObject.IconDisplayMode == GH_IconDisplayMode.icon){
                            docObject.IconDisplayMode = GH_IconDisplayMode.name;
                        }
                        else
                        {
                            docObject.IconDisplayMode = GH_IconDisplayMode.icon;
                        }
                        docObject.ExpireSolution(true);
                    }
                    Instances.RedrawCanvas();
                }
                else
                {
                    MessageBox.Show("Add at least one component.");
                }
            }
            else
            {
                MessageBox.Show("Canvas is null");
            }
        }

        private void OnSelectIcon_Click(object sender, EventArgs e)
        {
            if (Owner != null)
            {

                if (Owner.IsDocument)
                {
                    GH_Document doc = Owner.Document;
                    foreach (IGH_DocumentObject docObject in doc.SelectedObjects())
                    {
                        docObject.IconDisplayMode = GH_IconDisplayMode.icon;
                        docObject.ExpireSolution(true);
                    }
                    Instances.RedrawCanvas();
                }
                else
                {
                    MessageBox.Show("Add at least one component.");
                }
            }
            else
            {
                MessageBox.Show("Canvas is null");
            }
        }

        private void OnSelectApp_Click(object sender, EventArgs e)
        {
            if (Owner != null)
            {

                if (Owner.IsDocument)
                {
                    GH_Document doc = Owner.Document;
                    foreach (IGH_DocumentObject docObject in doc.SelectedObjects())
                    {
                        docObject.IconDisplayMode = GH_IconDisplayMode.application;
                        docObject.ExpireSolution(true);
                    }
                    Instances.RedrawCanvas();
                }
                else
                {
                    MessageBox.Show("Add at least one component.");
                }
            }
            else
            {
                MessageBox.Show("Canvas is null");
            }
        }

        private void OnCanvasCreated(GH_Canvas canvas)
        {
            Owner = canvas;

            
            
        }
    }
}

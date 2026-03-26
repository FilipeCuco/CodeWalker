using CodeWalker.GameFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeWalker.Project.Panels
{
    public partial class EditProjectManifestPanel : ProjectPanel
    {
        public ProjectForm ProjectForm { get; set; }
        public ProjectFile CurrentProjectFile { get; set; }

        public EditProjectManifestPanel(ProjectForm projectForm)
        {
            ProjectForm = projectForm;
            InitializeComponent();
            Tag = "_manifest.ymf";

            // Wire up live XML preview updates
            var grids = new[] { GridMapDataGroups, GridHDTxdBindings, GridImapDeps, GridImapDeps2, GridItypDeps2, GridInteriors };
            foreach (var grid in grids)
            {
                grid.CellValueChanged += Grid_CellValueChanged;
                grid.RowsRemoved += Grid_RowsRemoved;
                grid.CurrentCellDirtyStateChanged += Grid_CurrentCellDirtyStateChanged;
            }
        }

        private void Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            GenerateXmlFromTables();
        }

        private void Grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            GenerateXmlFromTables();
        }

        private void Grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Commit combo box changes immediately so CellValueChanged fires
            var grid = sender as DataGridView;
            if (grid != null && grid.IsCurrentCellDirty)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        public override void SetTheme(ThemeBase theme)
        {
            base.SetTheme(theme);

            var txtback = SystemColors.Window;
            var indback = Color.WhiteSmoke;

            if (theme is VS2015DarkTheme)
            {
                txtback = theme.ColorPalette.MainWindowActive.Background;
                indback = theme.ColorPalette.MainWindowActive.Background;
            }

            ProjectManifestTextBox.BackColor = txtback;
            ProjectManifestTextBox.IndentBackColor = indback;

            // Apply theme to grids
            var grids = new[] { GridMapDataGroups, GridHDTxdBindings, GridImapDeps, GridImapDeps2, GridItypDeps2, GridInteriors };
            foreach (var grid in grids)
            {
                if (theme is VS2015DarkTheme)
                {
                    grid.BackgroundColor = txtback;
                    grid.DefaultCellStyle.BackColor = txtback;
                    grid.DefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = indback;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.EnableHeadersVisualStyles = false;
                    grid.GridColor = Color.FromArgb(60, 60, 60);
                }
            }

            if (theme is VS2015DarkTheme)
            {
                StatusLabel.ForeColor = Color.White;
            }
        }


        public void SetProject(ProjectFile project)
        {
            CurrentProjectFile = project;
            GenerateXmlFromTables();
        }

        public void LoadYmfFile(YmfFile ymf)
        {
            if (ymf == null) return;

            // Clear all grids
            GridMapDataGroups.Rows.Clear();
            GridHDTxdBindings.Rows.Clear();
            GridImapDeps.Rows.Clear();
            GridImapDeps2.Rows.Clear();
            GridItypDeps2.Rows.Clear();
            GridInteriors.Rows.Clear();

            // MapDataGroups
            if (ymf.MapDataGroups != null)
            {
                foreach (var g in ymf.MapDataGroups)
                {
                    var name = g.Name.ToString();
                    var flags = g.Flags > 0 ? "INTERIOR_DATA" : "";
                    var hours = g.HoursOnOff.ToString();
                    var bounds = g.Bounds != null ? string.Join(", ", g.Bounds.Select(b => b.ToString())) : "";
                    var weather = g.WeatherTypes != null && g.WeatherTypes.Length > 0 ? g.WeatherTypes[0].ToString() : "";

                    // Ensure the hours value is in the combo box items
                    if (!ColMDG_HoursOnOff.Items.Contains(hours))
                        ColMDG_HoursOnOff.Items.Add(hours);

                    GridMapDataGroups.Rows.Add(name, flags, hours, bounds, weather);
                }
            }

            // HDTxdBindingArray
            if (ymf.HDTxdAssetBindings != null)
            {
                foreach (var h in ymf.HDTxdAssetBindings)
                {
                    string[] assetTypes = { "AT_TXD", "AT_DRB", "AT_DWD", "AT_FRG" };
                    var assetType = h.assetType < assetTypes.Length ? assetTypes[h.assetType] : "AT_TXD";
                    GridHDTxdBindings.Rows.Add(assetType, h.targetAsset.ToString(), h.HDTxd.ToString());
                }
            }

            // imapDependencies
            if (ymf.imapDependencies != null)
            {
                foreach (var d in ymf.imapDependencies)
                {
                    GridImapDeps.Rows.Add(d.imapName.ToString(), d.itypName.ToString(), d.packFileName.ToString());
                }
            }

            // imapDependencies_2
            if (ymf.imapDependencies2 != null)
            {
                foreach (var d in ymf.imapDependencies2)
                {
                    var flags = d.Dep.manifestFlags > 0 ? "INTERIOR_DATA" : "";
                    var deps = d.itypDepArray != null ? string.Join(", ", d.itypDepArray.Select(x => x.ToString())) : "";
                    GridImapDeps2.Rows.Add(d.Dep.imapName.ToString(), flags, deps);
                }
            }

            // itypDependencies_2
            if (ymf.itypDependencies2 != null)
            {
                foreach (var d in ymf.itypDependencies2)
                {
                    var flags = d.Dep.manifestFlags > 0 ? "INTERIOR_DATA" : "";
                    var deps = d.itypDepArray != null ? string.Join(", ", d.itypDepArray.Select(x => x.ToString())) : "";
                    GridItypDeps2.Rows.Add(d.Dep.itypName.ToString(), flags, deps);
                }
            }

            // Interiors
            if (ymf.Interiors != null)
            {
                foreach (var i in ymf.Interiors)
                {
                    var bounds = i.Bounds != null ? string.Join(", ", i.Bounds.Select(b => b.ToString())) : "";
                    GridInteriors.Rows.Add(i.Interior.Name.ToString(), bounds);
                }
            }

            GenerateXmlFromTables();
            Text = ymf.FileEntry?.Name ?? "_manifest.ymf";
            StatusLabel.Text = "Loaded " + (ymf.FileEntry?.Name ?? "ymf file");
        }


        private string CellStr(DataGridViewRow row, int colIndex)
        {
            var val = row.Cells[colIndex].Value;
            return val?.ToString()?.Trim() ?? "";
        }

        private string[] SplitComma(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return Array.Empty<string>();
            return s.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
        }


        private void GenerateXmlFromTables()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
            sb.AppendLine("<CPackFileMetaData>");

            // === MapDataGroups ===
            GenerateMapDataGroups(sb);

            // === HDTxdBindingArray ===
            GenerateHDTxdBindings(sb);

            // === imapDependencies ===
            GenerateImapDependencies(sb);

            // === imapDependencies_2 ===
            GenerateImapDependencies2(sb);

            // === itypDependencies_2 ===
            GenerateItypDependencies2(sb);

            // === Interiors ===
            GenerateInteriors(sb);

            sb.AppendLine("</CPackFileMetaData>");
            ProjectManifestTextBox.Text = sb.ToString();
            Text = "_manifest.ymf*";
        }

        private void GenerateMapDataGroups(StringBuilder sb)
        {
            var rows = GetDataRows(GridMapDataGroups);
            if (rows.Count == 0)
            {
                sb.AppendLine("  <MapDataGroups/>");
                return;
            }
            sb.AppendLine("  <MapDataGroups>");
            foreach (var row in rows)
            {
                var name = CellStr(row, 0);
                var flags = CellStr(row, 1);
                var hours = CellStr(row, 2);
                var bounds = SplitComma(CellStr(row, 3));
                var weather = SplitComma(CellStr(row, 4));

                if (string.IsNullOrEmpty(name)) continue;

                sb.AppendLine("    <Item>");
                sb.AppendLine("      <Name>" + name + "</Name>");
                if (flags.Length > 0)
                    sb.AppendLine("      <Flags value=\"" + flags + "\"/>");
                else
                    sb.AppendLine("      <Flags value=\"0\"/>");
                if (hours.Length > 0)
                    sb.AppendLine("      <HoursOnOff value=\"" + hours + "\"/>");
                else
                    sb.AppendLine("      <HoursOnOff value=\"0\"/>");

                if (bounds.Length > 0)
                {
                    sb.AppendLine("      <Bounds>");
                    foreach (var b in bounds)
                        sb.AppendLine("        <Item>" + b + "</Item>");
                    sb.AppendLine("      </Bounds>");
                }
                else
                {
                    sb.AppendLine("      <Bounds/>");
                }

                if (weather.Length > 0)
                {
                    sb.AppendLine("      <WeatherTypes>");
                    foreach (var w in weather)
                        sb.AppendLine("        <Item>" + w + "</Item>");
                    sb.AppendLine("      </WeatherTypes>");
                }
                else
                {
                    sb.AppendLine("      <WeatherTypes/>");
                }

                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </MapDataGroups>");
        }

        private void GenerateHDTxdBindings(StringBuilder sb)
        {
            var rows = GetDataRows(GridHDTxdBindings);
            if (rows.Count == 0)
            {
                sb.AppendLine("  <HDTxdBindingArray/>");
                return;
            }
            sb.AppendLine("  <HDTxdBindingArray>");
            foreach (var row in rows)
            {
                var assetType = CellStr(row, 0);
                var target = CellStr(row, 1);
                var hdtxd = CellStr(row, 2);
                if (string.IsNullOrEmpty(target) && string.IsNullOrEmpty(hdtxd)) continue;

                sb.AppendLine("    <Item>");
                sb.AppendLine("      <assetType>" + (assetType.Length > 0 ? assetType : "AT_TXD") + "</assetType>");
                sb.AppendLine("      <targetAsset>" + target + "</targetAsset>");
                sb.AppendLine("      <HDTxd>" + hdtxd + "</HDTxd>");
                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </HDTxdBindingArray>");
        }

        private void GenerateImapDependencies(StringBuilder sb)
        {
            var rows = GetDataRows(GridImapDeps);
            if (rows.Count == 0)
            {
                sb.AppendLine("  <imapDependencies/>");
                return;
            }
            sb.AppendLine("  <imapDependencies>");
            foreach (var row in rows)
            {
                var imap = CellStr(row, 0);
                var ityp = CellStr(row, 1);
                var pack = CellStr(row, 2);
                if (string.IsNullOrEmpty(imap)) continue;

                sb.AppendLine("    <Item>");
                sb.AppendLine("      <imapName>" + imap + "</imapName>");
                sb.AppendLine("      <itypName>" + ityp + "</itypName>");
                sb.AppendLine("      <packFileName>" + pack + "</packFileName>");
                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </imapDependencies>");
        }

        private void GenerateImapDependencies2(StringBuilder sb)
        {
            var rows = GetDataRows(GridImapDeps2);
            if (rows.Count == 0)
            {
                sb.AppendLine("  <imapDependencies_2/>");
                return;
            }
            sb.AppendLine("  <imapDependencies_2>");
            foreach (var row in rows)
            {
                var imap = CellStr(row, 0);
                var flags = CellStr(row, 1);
                var deps = SplitComma(CellStr(row, 2));
                if (string.IsNullOrEmpty(imap)) continue;

                sb.AppendLine("    <Item>");
                sb.AppendLine("      <imapName>" + imap + "</imapName>");
                if (flags.Length > 0)
                    sb.AppendLine("      <manifestFlags>" + flags + "</manifestFlags>");
                else
                    sb.AppendLine("      <manifestFlags/>");
                if (deps.Length > 0)
                {
                    sb.AppendLine("      <itypDepArray>");
                    foreach (var d in deps)
                        sb.AppendLine("        <Item>" + d + "</Item>");
                    sb.AppendLine("      </itypDepArray>");
                }
                else
                {
                    sb.AppendLine("      <itypDepArray/>");
                }
                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </imapDependencies_2>");
        }

        private void GenerateItypDependencies2(StringBuilder sb)
        {
            var rows = GetDataRows(GridItypDeps2);
            if (rows.Count == 0)
            {
                sb.AppendLine("  <itypDependencies_2/>");
                return;
            }
            sb.AppendLine("  <itypDependencies_2>");
            foreach (var row in rows)
            {
                var ityp = CellStr(row, 0);
                var flags = CellStr(row, 1);
                var deps = SplitComma(CellStr(row, 2));
                if (string.IsNullOrEmpty(ityp)) continue;

                sb.AppendLine("    <Item>");
                sb.AppendLine("      <itypName>" + ityp + "</itypName>");
                if (flags.Length > 0)
                    sb.AppendLine("      <manifestFlags>" + flags + "</manifestFlags>");
                else
                    sb.AppendLine("      <manifestFlags/>");
                if (deps.Length > 0)
                {
                    sb.AppendLine("      <itypDepArray>");
                    foreach (var d in deps)
                        sb.AppendLine("        <Item>" + d + "</Item>");
                    sb.AppendLine("      </itypDepArray>");
                }
                else
                {
                    sb.AppendLine("      <itypDepArray/>");
                }
                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </itypDependencies_2>");
        }

        private void GenerateInteriors(StringBuilder sb)
        {
            var rows = GetDataRows(GridInteriors);
            if (rows.Count == 0)
            {
                sb.AppendLine("  <Interiors/>");
                return;
            }
            sb.AppendLine("  <Interiors itemType=\"CInteriorBoundsFiles\">");
            foreach (var row in rows)
            {
                var name = CellStr(row, 0);
                var bounds = SplitComma(CellStr(row, 1));
                if (string.IsNullOrEmpty(name)) continue;

                sb.AppendLine("    <Item>");
                sb.AppendLine("      <Name>" + name + "</Name>");
                if (bounds.Length > 0)
                {
                    sb.AppendLine("      <Bounds>");
                    foreach (var b in bounds)
                        sb.AppendLine("        <Item>" + b + "</Item>");
                    sb.AppendLine("      </Bounds>");
                }
                else
                {
                    sb.AppendLine("      <Bounds>");
                    sb.AppendLine("        <Item>" + name + "</Item>");
                    sb.AppendLine("      </Bounds>");
                }
                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </Interiors>");
        }


        private List<DataGridViewRow> GetDataRows(DataGridView grid)
        {
            var result = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                bool hasData = false;
                for (int c = 0; c < row.Cells.Count; c++)
                {
                    if (!string.IsNullOrWhiteSpace(row.Cells[c].Value?.ToString()))
                    {
                        hasData = true;
                        break;
                    }
                }
                if (hasData) result.Add(row);
            }
            return result;
        }


        /// <summary>
        /// Auto-populates imapDependencies_2, itypDependencies_2, and Interiors
        /// from the current project's ymap and ytyp files.
        /// </summary>
        private void AutoPopulateFromProject()
        {
            if (CurrentProjectFile == null)
            {
                CurrentProjectFile = ProjectForm?.CurrentProjectFile;
            }
            if (CurrentProjectFile == null) return;

            var getYtypName = new Func<YtypFile, string>((ytyp) =>
            {
                var ytypname = ytyp?.RpfFileEntry?.NameLower;
                if (ytyp != null)
                {
                    if (string.IsNullOrEmpty(ytypname))
                    {
                        ytypname = ytyp.RpfFileEntry?.Name?.ToLowerInvariant() ?? "";
                    }
                    if (ytypname.EndsWith(".ytyp"))
                    {
                        ytypname = ytypname.Substring(0, ytypname.Length - 5);
                    }
                }
                return ytypname;
            });

            var typdeps = new Dictionary<string, Dictionary<string, YtypFile>>();
            var interiors = new List<string>();

            // Process YMAPs -> imapDependencies_2
            if (CurrentProjectFile.YmapFiles.Count > 0)
            {
                GridImapDeps2.Rows.Clear();

                foreach (var ymap in CurrentProjectFile.YmapFiles)
                {
                    var ymapname = ymap.RpfFileEntry?.NameLower;
                    if (string.IsNullOrEmpty(ymapname))
                    {
                        ymapname = ymap.Name.ToLowerInvariant();
                    }
                    if (ymapname.EndsWith(".ymap"))
                    {
                        ymapname = ymapname.Substring(0, ymapname.Length - 5);
                    }

                    var mapdeps = new Dictionary<string, YtypFile>();
                    bool ismilo = false;
                    if (ymap.AllEntities != null)
                    {
                        foreach (var ent in ymap.AllEntities)
                        {
                            var ytyp = ent.Archetype?.Ytyp;
                            var ytypname = getYtypName(ytyp);
                            if (ytyp != null)
                            {
                                mapdeps[ytypname] = ytyp;
                            }
                            if (ent.IsMlo)
                            {
                                ismilo = true;
                                if (ent.MloInstance?.Entities != null)
                                {
                                    if (!typdeps.TryGetValue(ytypname, out var typdepdict))
                                    {
                                        typdepdict = new Dictionary<string, YtypFile>();
                                        typdeps[ytypname] = typdepdict;
                                    }
                                    foreach (var ient in ent.MloInstance.Entities)
                                    {
                                        var iytyp = ient.Archetype?.Ytyp;
                                        var iytypname = getYtypName(iytyp);
                                        if ((iytyp != null) && (iytypname != ytypname))
                                        {
                                            typdepdict[iytypname] = iytyp;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (ymap.GrassInstanceBatches != null)
                    {
                        foreach (var batch in ymap.GrassInstanceBatches)
                        {
                            var ytyp = batch.Archetype?.Ytyp;
                            var ytypname = getYtypName(ytyp);
                            if (ytyp != null)
                            {
                                mapdeps[ytypname] = ytyp;
                            }
                        }
                    }

                    var depsStr = string.Join(", ", mapdeps.Keys);
                    var flagsStr = ismilo ? "INTERIOR_DATA" : "";

                    GridImapDeps2.Rows.Add(ymapname, flagsStr, depsStr);
                }
            }

            // Process YTYPs -> itypDependencies_2, Interiors
            if ((CurrentProjectFile.YtypFiles.Count > 0) && (ProjectForm?.GameFileCache != null))
            {
                foreach (var ytyp in CurrentProjectFile.YtypFiles)
                {
                    var ytypname = getYtypName(ytyp);
                    foreach (var archm in ytyp.AllArchetypes)
                    {
                        var mloa = archm as MloArchetype;
                        if (mloa != null)
                        {
                            interiors.Add(mloa.Name);
                            if (!typdeps.TryGetValue(ytypname, out var typdepdict))
                            {
                                typdepdict = new Dictionary<string, YtypFile>();
                                typdeps[ytypname] = typdepdict;
                            }
                            if (mloa.entities != null)
                            {
                                foreach (var ent in mloa.entities)
                                {
                                    var archname = ent._Data.archetypeName;
                                    var arch = ProjectForm.GameFileCache.GetArchetype(archname);
                                    var iytyp = arch?.Ytyp;
                                    var iytypname = getYtypName(iytyp);
                                    if ((iytyp != null) && (iytypname != ytypname))
                                    {
                                        typdepdict[iytypname] = iytyp;
                                    }
                                }
                            }
                            if (mloa.entitySets != null)
                            {
                                foreach (var entset in mloa.entitySets)
                                {
                                    if (entset.Entities != null)
                                    {
                                        foreach (var ent in entset.Entities)
                                        {
                                            var archname = ent._Data.archetypeName;
                                            var arch = ProjectForm.GameFileCache.GetArchetype(archname);
                                            var iytyp = arch?.Ytyp;
                                            var iytypname = getYtypName(iytyp);
                                            if ((iytyp != null) && (iytypname != ytypname))
                                            {
                                                typdepdict[iytypname] = iytyp;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Populate itypDependencies_2 grid
            if (typdeps.Count > 0)
            {
                GridItypDeps2.Rows.Clear();
                foreach (var kvp1 in typdeps)
                {
                    var depsStr = string.Join(", ", kvp1.Value.Keys);
                    GridItypDeps2.Rows.Add(kvp1.Key, "INTERIOR_DATA", depsStr);
                }
            }

            // Populate Interiors grid
            if (interiors.Count > 0)
            {
                GridInteriors.Rows.Clear();
                foreach (var interior in interiors)
                {
                    GridInteriors.Rows.Add(interior, interior);
                }
            }

            // Regenerate XML with new data
            GenerateXmlFromTables();
        }


        private void ProjectManifestGenerateButton_Click(object sender, EventArgs e)
        {
            CurrentProjectFile = ProjectForm.CurrentProjectFile;
            GenerateXmlFromTables();
            StatusLabel.Text = "XML generated from tables.";
        }

        private void AutoPopulateButton_Click(object sender, EventArgs e)
        {
            CurrentProjectFile = ProjectForm.CurrentProjectFile;
            AutoPopulateFromProject();
            StatusLabel.Text = "Tables populated from project. XML generated.";
        }

        private void SaveManifestButton_Click(object sender, EventArgs e)
        {
            // Regenerate from tables to ensure XML is up to date
            GenerateXmlFromTables();

            if (SaveFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                var filename = SaveFileDialog.FileName;
                var xml = ProjectManifestTextBox.Text;
                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(xml);
                var pso = XmlPso.GetPso(xmldoc);
                var bytes = pso.Save();
                File.WriteAllBytes(filename, bytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving _manifest.ymf file:\n" + ex.ToString());
            }
        }
    }
}

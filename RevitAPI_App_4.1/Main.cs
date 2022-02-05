using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_lab4._1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            TaskDialog.Show("Запись данных", $"Данное приложение запишет данные о стенах, используемых в проекте. " +
                $"{ Environment.NewLine} Файл будет создан на Рабочем столе, имя файла wallInfo.csv" +
                $"{ Environment.NewLine} После выполнения появится диалоговое окно с сообщением");

            string wallInfo = string.Empty;

            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();

            foreach (Wall wall in walls)
            {
                double wallVolume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                string wallVolumeM = UnitUtils.ConvertFromInternalUnits(wallVolume, UnitTypeId.CubicMeters).ToString();
                wallInfo += $"{wall.Name}\t{wallVolumeM}{Environment.NewLine}";
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string csvPath = Path.Combine(desktopPath, "wallInfo.csv");

            File.WriteAllText(csvPath, wallInfo);

            TaskDialog.Show("Запись данных", $"{ Environment.NewLine} Данные записаны в файл wallInfo.csv, на Рабочем столе" +
              $"{ Environment.NewLine} Нажмите кнопку Закрыть для завершения работы приложения");
            return Result.Succeeded;
        }
    }
}
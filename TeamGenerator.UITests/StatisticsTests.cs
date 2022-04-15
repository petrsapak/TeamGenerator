using System;
using System.Threading;
using FlaUI.Core;
using NUnit.Framework;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using TeamGenerator.AutomationIds;

namespace TeamGenerator.UITests
{
    [TestFixture]
    public class StatisticsTests
    {
        private Application application;
        private Window window;
        private UIA3Automation automation;
        private string applicationPath = @"C:\Private\TeamGenerator\bin\TeamGenerator.exe";
        private string statisticsTestFilePath = @"C:\Private\TeamGenerator\Data\Statistics\26-3-2022.tgst";

        public StatisticsTests()
        {
            automation = new UIA3Automation();
            application = Application.Launch(applicationPath);
            window = application.GetMainWindow(automation);
        }

        [Test]
        public void Test()
        {
            var statisticsButton = window.FindFirstDescendant(conditionFactory =>
                conditionFactory.ByAutomationId(MainMenuAutomationIds.NavigateToStatisticsButton))?.AsButton();

            statisticsButton?.Invoke();

            var lastMatchesListView = window.FindFirstDescendant(conditionFactory =>
                conditionFactory.ByAutomationId(StatisticsAutomationIdentifiers.LastMatchesListView))?.AsListBox();
            Assert.That(lastMatchesListView?.Items.Length, Is.EqualTo(0));

            var loadMatchesButton = window.FindFirstDescendant(conditionFactory =>
                conditionFactory.ByAutomationId(StatisticsAutomationIdentifiers.LoadMatchesButton))?.AsButton();
            loadMatchesButton?.Invoke();
            Keyboard.Type(statisticsTestFilePath);

            var cancelButton = window.FindFirstDescendant(conditionFactory =>
                conditionFactory.ByName("Close"))?.AsButton();

            cancelButton?.Invoke();

            lastMatchesListView = window.FindFirstDescendant(conditionFactory =>
                conditionFactory.ByAutomationId(StatisticsAutomationIdentifiers.LastMatchesListView))?.AsListBox();
            Assert.That(lastMatchesListView?.Items.Length, Is.EqualTo(5));
        }
    }
}
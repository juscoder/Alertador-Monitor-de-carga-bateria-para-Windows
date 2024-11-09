using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media; // Necesario para reproducir sonido

namespace BatteryMonitor
{
    public partial class form1 : Form
    {
        private int batteryLimit = 90; // Valor predeterminado
        private Timer checkBatteryTimer;

        public form1()
        {
            InitializeComponent();

            // Configura el valor inicial en el NumericUpDown de la UI
            numericUpDownLimit.Value = batteryLimit;

            // Configuración del Timer para verificar el nivel de batería cada minuto
            checkBatteryTimer = new Timer();
            checkBatteryTimer.Interval = 60000; // 60,000 ms = 1 minuto
            checkBatteryTimer.Tick += CheckBatteryLevel;
            checkBatteryTimer.Start();

            // Llama a UpdateBatteryPercentage() para mostrar el porcentaje de batería al abrir el programa
            UpdateBatteryPercentage();
        }

        // Método para verificar el nivel de batería y mostrar una alerta si se alcanza el límite
        private void CheckBatteryLevel(object sender, EventArgs e)
        {
            UpdateBatteryPercentage(); // Actualiza el porcentaje en el Label cada vez que el Timer se activa

            var batteryStatus = SystemInformation.PowerStatus;
            int batteryLevel = (int)(batteryStatus.BatteryLifePercent * 100);

            if (batteryStatus.PowerLineStatus == PowerLineStatus.Online && batteryLevel >= batteryLimit)
            {
                // Reproduce un sonido personalizado en bucle continuo
                SoundPlayer player = new SoundPlayer(@"C:\Users\Equipo\Downloads\siren-alert-96052.wav");
                try
                {
                    player.PlayLooping(); // Reproduce el sonido en un bucle infinito
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al reproducir el sonido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Muestra el mensaje de advertencia y espera a que el usuario cierre el cuadro de diálogo
                MessageBox.Show("Desconecta el cargador, la batería ha alcanzado el límite de carga.", "Alerta de Batería", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Detiene el sonido una vez que se cierra el cuadro de mensaje
                player.Stop();
            }
        }




        // Método para actualizar el porcentaje de batería en el Label
        private void UpdateBatteryPercentage()
        {
            var batteryStatus = SystemInformation.PowerStatus;
            int batteryLevel = (int)(batteryStatus.BatteryLifePercent * 100);
            lblBatteryPercentage.Text = $"Porcentaje de Batería: {batteryLevel}%";
        }

        // Método que se ejecuta al hacer clic en el botón de guardar
        private void btnSave_Click(object sender, EventArgs e)
        {
            batteryLimit = (int)numericUpDownLimit.Value;
            MessageBox.Show($"Límite de carga configurado al {batteryLimit}%", "Configuración Guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

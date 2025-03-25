namespace ClinicService.BLL.RabbitMqProducer;
public interface IRabbitMqService
{
    void SendMessage(object obj);
    void SendMessage(string message);
}

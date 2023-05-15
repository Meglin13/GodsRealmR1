/// <summary>
/// Интерфейс для интерактивных объектов. Обязательно должен присутствовать коллайдер
/// </summary>
internal interface IInteractable
{
    bool CanInteract();
    void Interaction();
}
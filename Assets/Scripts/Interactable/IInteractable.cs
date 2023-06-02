using UnityEngine;
/// <summary>
/// Интерфейс для интерактивных объектов. Обязательно должен присутствовать коллайдер
/// </summary>
internal interface IInteractable
{
    GameObject gameObject { get; }
    bool CanInteract();
    void Interaction();
}
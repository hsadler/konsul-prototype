using UnityEngine;
using UnityEngine.Events;

public class FactoryStructureSelectedEvent : UnityEvent<GameObject> { }

public class FactoryStructureDelesectAllEvent : UnityEvent { }

public class FactoryStructureRemovalEvent : UnityEvent<GameObject> { }

public class FactoryStructureIOPlacementEvent : UnityEvent<GameObject, GameObject> { }

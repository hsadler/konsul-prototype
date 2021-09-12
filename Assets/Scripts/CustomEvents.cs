using UnityEngine;
using UnityEngine.Events;

public class FactoryEntitySelectedEvent : UnityEvent<GameObject> { }

public class FactoryEntityDelesectAllEvent : UnityEvent { }

public class FactoryStructureIODelesectAllEvent : UnityEvent { }

public class FactoryStructureIOPlacementEvent : UnityEvent<GameObject, GameObject> { }

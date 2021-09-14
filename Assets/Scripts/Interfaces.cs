using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactoryEntity
{
    int FactoryEntityType { get; set; }
    int LauncherGameObjectId { get; set; }
    bool InTransit { get; set; }
    string GetStringFormattedFactoryEntityInfo();
}

public interface IInTransitFE { }


// factory-structure group and interfaces
public interface IFactoryStructure
{
    bool IsStructureActive { get; set; }
}

public interface IFactoryHarvester { }

public interface IFactoryDistributor { }

public interface IFactoryStorage { }

public interface IFactoryRawResourceProcessor { }

// factory-unit group and interfaces
public interface IFactoryUnit { }

public interface IFactoryWorker { }
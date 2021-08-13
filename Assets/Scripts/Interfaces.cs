using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactoryEntity
{
    int FactoryEntityType { get; set; }
    // TODO: consider moving LauncherGameObjectId to the lauchable script since it's responsible for it
    int LauncherGameObjectId { get; set; }
    string GetStringFormattedFactoryEntityInfo();
}

// factory-resource group and interfaces
public interface IFactoryResource
{
}

// factory-structure group and interfaces
public interface IFactoryStructure
{
    bool IsStructureActive { get; set; }
}

public interface IFactoryHarvester
{
}

public interface IFactoryDistributor
{
}

public interface IFactoryStorage
{
    void AdminPopulateStorage();
}

// factory-unit group and interfaces
public interface IFactoryUnit
{
}

public interface IFactoryWorker
{
}
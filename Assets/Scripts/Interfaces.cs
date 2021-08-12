using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactoryEntity
{
    int FactoryEntityType { get; }
    string GetStringFormattedFactoryEntityInfo();
}

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

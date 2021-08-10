using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactoryStructure
{
    string GetStringFormattedFactoryStructureInfo();
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

using System.Collections.Generic;
using UnityEngine; 
public class MasterDataAsset : ScriptableObject 
{ 
public List<LocalizeArticleData> LocalizeArticleData = new List<LocalizeArticleData>();
public List<LocalizeConstructionData> LocalizeConstructionData = new List<LocalizeConstructionData>();
public List<LocalizeRolesData> LocalizeRolesData = new List<LocalizeRolesData>();
public List<LocalizeSkillsData> LocalizeSkillsData = new List<LocalizeSkillsData>();
public List<LocalizeUIDialogData> LocalizeUIDialogData = new List<LocalizeUIDialogData>();
 
} 

using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpookSuite.Handler
{
    public class ChamHandler
    {
        //private static Dictionary<int, List<Renderer>> renderers = new Dictionary<int, List<Renderer>>();
        private static Dictionary<int, Material[]> materials = new Dictionary<int, Material[]>();
        public static Material m_chamMaterial;
        private static int _color;
        public static bool overrideMaterial;
        public static Material overridedMaterial;

        private Object @object;

        public ChamHandler(Object obj)
        {
            @object = obj;
        }

        public static void SetupChamMaterial()
        {
            m_chamMaterial = overrideMaterial ? overridedMaterial : new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy
            };
                m_chamMaterial.SetInt("_SrcBlend", 5);
                m_chamMaterial.SetInt("_DstBlend", 10);
                m_chamMaterial.SetInt("_Cull", 0);
                m_chamMaterial.SetInt("_ZTest", 8);
                m_chamMaterial.SetInt("_ZWrite", 0);
                m_chamMaterial.SetColor("_Color", Settings.c_chamItems.GetColor());
                _color = Shader.PropertyToID("_Color");

            SpookSuite.Instance.StartCoroutine(CleanUpMaterials());
        }

        private List<Renderer> GetRenderers()
        {
            List<Renderer> renderers = new List<Renderer>();

            if (@object == null) return renderers;


            if (@object is GameObject) renderers.AddRange(((GameObject)@object).GetComponentsInChildren<Renderer>());
            if (@object is Component) renderers.AddRange(((Component)@object).GetComponentsInChildren<Renderer>());
            return renderers;
        }

        public void RemoveCham()
        {
            GetRenderers().ForEach(r =>
            {
                if (materials.ContainsKey(r.GetInstanceID()))
                {
                    r.SetMaterials(materials[r.GetInstanceID()].ToList());
                    materials.Remove(r.GetInstanceID());
                }
            });
        }

        public void ProcessCham(float distance, RGBAColor color)
        {
            if (@object == null) return;

            bool e = false;

            if (@object is Player && !((Player)@object).ai) e = ChamESP.displayPlayers;
            if (@object is Bot || (@object is Player && ((Player)@object).ai)) e = ChamESP.displayEnemies;
            if (@object is Pickup) e = ChamESP.displayItems;
            if (@object is UseDivingBellButton || @object is DivingBell) e = ChamESP.displayDivingBell;
            if (@object is Laser) e = ChamESP.displayLasers;
            m_chamMaterial = overrideMaterial ? overridedMaterial : m_chamMaterial;
            if (e && distance >= ChamESP.Value && Cheat.Instance<ChamESP>().Enabled) ApplyCham(color);
            else RemoveCham();
        }

        public void ApplyCham(RGBAColor color)
        {
            if (@object == null) return;

            GetRenderers().ForEach(r =>
            {
                if (r == null) return;
                m_chamMaterial = overrideMaterial ? overridedMaterial : m_chamMaterial;
                if (!materials.ContainsKey(r.GetInstanceID()))
                {
                    if (r.materials == null) return;
                    materials.Add(r.GetInstanceID(), r.materials);
                    r.SetMaterials(Enumerable.Repeat(m_chamMaterial, r.materials.Length).ToList());
                }
                if (!overridedMaterial)
                    UpdateChamColor(r, color);
            });
        }

        private void UpdateChamColor(Renderer r, RGBAColor color)
        {
            if (r == null || r.materials == null) return;
            r.materials.ToList().ForEach(m => m.SetColor(_color, color.GetColor()));
        }

        public static ChamHandler GetHandler(Object obj)
        {
            return new ChamHandler(obj);
        }

        private static IEnumerator CleanUpMaterials()
        {
            while (true)
            {
                yield return new WaitForSeconds(15f);
                int cnt = 0;
                List<int> keep = new List<int>();
                Object.FindObjectsOfType<Renderer>().ToList().ForEach(r => keep.Add(r.GetInstanceID()));

                materials.Keys.ToList().FindAll(k => !keep.Contains(k)).ForEach(k => { materials.Remove(k); cnt++; });

            }
        }
    }

    public static class RendererExtensions
    {
        public static ChamHandler GetChamHandler(this Object obj)
        {
            return ChamHandler.GetHandler(obj);
        }
    }
}

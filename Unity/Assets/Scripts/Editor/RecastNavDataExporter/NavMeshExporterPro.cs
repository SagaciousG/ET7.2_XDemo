using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ET;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ETEditor
{
    public class NavMeshExporterPro
    {
        private static string outputClientFolder = "../Tools/RecastNavExportor/Meshes/";
        
        [MenuItem("Tools/Explore Navmesh")]
        public static void Explore()
        {
            var obj = new GameObject();
            var meshFilter = obj.AddComponent<MeshFilter>();
            var mr = obj.AddComponent<MeshRenderer>();
            mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            var triangulation = NavMesh.CalculateTriangulation();
            
            meshFilter.sharedMesh = new Mesh();
            meshFilter.sharedMesh.vertices = triangulation.vertices;
            meshFilter.sharedMesh.triangles = triangulation.indices;
            
            if (!System.IO.Directory.Exists(outputClientFolder))
            {
                System.IO.Directory.CreateDirectory(outputClientFolder);
            }

            var filename = SceneManager.GetActiveScene().name;
            var path = outputClientFolder + filename + ".obj";
            StreamWriter sw = new StreamWriter(path);

            try
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");
                string strMes = MeshToString(meshFilter);
                sw.Write(strMes);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            finally
            {
                sw.Close();
                Object.DestroyImmediate(obj);
            }
        }
        
        public static string MeshToString(MeshFilter mf)
        {
            Mesh m = mf.sharedMesh;
            if (m == null)
                return "";
            Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;
            StringBuilder sb = new StringBuilder();
            sb.Append("g ").Append(mf.name).Append("\n");
            // foreach(Vector3 v in m.vertices) {
            // 	sb.Append(string.Format("v {0} {1} {2}\n",v.x,v.y,v.z));
            // }
            foreach (Vector3 lv in m.vertices)
            {
                Vector3 wv = mf.transform.TransformPoint(lv);
                //This is sort of ugly - inverting x-component since we're in
                //a different coordinate system than "everyone" is "used to".
                sb.Append(string.Format("v {0} {1} {2}\n", -wv.x, wv.y, wv.z));
            }

            sb.Append("\n");

            // foreach(Vector3 v in m.normals) {
            // 	sb.Append(string.Format("vn {0} {1} {2}\n",v.x,v.y,v.z));
            // }
            foreach (Vector3 lv in m.normals)
            {
                Vector3 wv = mf.transform.TransformDirection(lv);
                sb.Append(string.Format("vn {0} {1} {2}\n", -wv.x, wv.y, wv.z));
            }

            sb.Append("\n");

            foreach (Vector3 v in m.uv)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }

            int countMat = m.subMeshCount;
            if (mats == null)
            {
                Debug.LogWarning($"NavMeshExporter MeshToString Error - 没有找到材质");
                return sb.ToString();
            }
            else if (mats.Length < countMat)
            {
                Debug.LogWarning($"NavMeshExporter MeshToString Error - 共享材质数量小于该物体的子物体数量 - {mats.Length} / {countMat}");
                countMat = mats.Length;
            }

            for (int material = 0; material < countMat; material++)
            {
                string nameMat = "null";
                Texture mainTexture = null;
                if (mats[material] != null)
                {
                    nameMat = mats[material].name;
                    mainTexture = mats[material].mainTexture;
                }

                sb.Append("\n");
                sb.Append("usemtl ").Append(nameMat).Append("\n");
                sb.Append("usemap ").Append(nameMat).Append("\n");

                //See if this material is already in the materiallist.
                try
                {
                    NavMeshExporter.ObjMaterial objMaterial = new NavMeshExporter.ObjMaterial();
                    objMaterial.name = nameMat;
                    if (mainTexture)
                        objMaterial.textureName = AssetDatabase.GetAssetPath(mainTexture);
                    else
                        objMaterial.textureName = null;
                }
                catch (ArgumentException)
                {
                    //Already in the dictionary
                }

                // int[] triangles = m.GetTriangles(material);
                // for (int i=0;i<triangles.Length;i+=3) {
                // 	sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", 
                // 		triangles[i]+1, triangles[i+1]+1, triangles[i+2]+1));
                // }
                int[] triangles = m.GetTriangles(material);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    //Because we inverted the x-component, we also needed to alter the triangle winding.
                    sb.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n",
                        triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
                }
            }

            return sb.ToString();
        }
    }
}
//using UnityEngine;
//using System.Threading;
//using System.Collections.Generic;
//using UnityEngine.Rendering;
//using System.Threading.Tasks;
//using Unity.Mathematics;

//public class GameManager : MonoBehaviour
//{
//    public int networks = 50;
//    List<Thread> threads = new List<Thread>();
//	public GameObject car;
//	List<GameObject> cars = new List<GameObject>();
//	public GameObject[] checkpoints;
//    public NetworkManager networkManager;
//	public LayerMask layermask;
//	public float runtime = 25f;
//	public float gameTime = 0;
//	public int generation = 0;
//	public void Start()
//	{
		
//       networkManager = new NetworkManager(5, new int[] {2, 4}, 2, networks, 0.05f, 0.1f, 0.05f);
//	}
//	public void reStart()
//	{
//		destroyCarOfTag("car");		
//		for (int i = 0; i < networks; i++)
//		{
//			cars.Add(Instantiate(car, transform));
//			cars[i].GetComponent<CollisionCheck>().format(i, this);
//			cars[i].tag = "car";
//		}
//		gameTime = 0;
//	}
//	public void Update()
//	{
//		generation = networkManager.generation;
//		for (int i = 0; i < cars.Count; i++)
//		{
//			InputStuff(i);
//		}
//		gameTime += Time.deltaTime;
//		if (gameTime > runtime)
//		{
//			networkManager.MakeNewGen();
//			reStart();
//		}
//		else if (networkManager.isAllDead())
//		{
//			networkManager.MakeNewGen();
//			reStart();
//		}
//		//(cars[0]);
//	} 
//	void InputStuff(int i)
//	{
//		List<double> ins = new List<double>();
//		for (int j = -90; j <= 90; j += 45)
//		{
//			RaycastHit rch = new RaycastHit();
//			Physics.Raycast(cars[i].transform.position, Quaternion.Euler(0, j, 0) * cars[i].transform.up, out rch);
//			//Debug.DrawRay(cars[i].transform.position, Quaternion.Euler(j, 0, 0) * cars[i].transform.up, Color.red, 0.1f);
//			//Debug.DrawRay(cars[i].transform.position, Quaternion.Euler(0, j, 0) * cars[i].transform.up, Color.green, 0f); correct
//			//Debug.DrawRay(cars[i].transform.position, Quaternion.Euler(0, 0, j) * cars[i].transform.up, Color.blue, 0.1f);
//			ins.Add(1/rch.distance);
//		}
//		double[] outputs = networkManager.feedOne(ins.ToArray(), i);
//		if (outputs == null)
//		{	
//			//print("its null");
//			return;
//		}
//		//Debug.LogWarning(i + "\nmove: " + outputs[0] + "\nturn: " + outputs[1]);
//		cars[i].transform.Rotate(0, 0, (float)outputs[1]*5);
//		cars[i].transform.Translate(cars[i].transform.forward*(float)outputs[0]);
//	}
//	public void killCar(int index)
//	{
//		networkManager.neuralNetwork[index].addScore(-1d);
//		networkManager.neuralNetwork[index].active = false;
//	}
//	public void giveOneScore(int index, int score)
//	{
//		networkManager.giveOneScore(score, index);
//	}
//	public void destroyCarOfTag(string tag)
//	{
//		GameObject[] gos = GameObject. FindGameObjectsWithTag(tag); foreach(GameObject go in gos) {cars.Remove(go); Destroy(go);};
//	}
//}

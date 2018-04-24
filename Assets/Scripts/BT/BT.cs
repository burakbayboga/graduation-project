using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BT {

	public BTNode root;
	public GCS unit;
	public int emptySlot;

	public void Initialize(){

		//GenerateBTv010();
		if(unit.unitType == "spotter"){
			GenerateSpotterBTv100();
		}
		else if(unit.unitType == "marine"){
			GenerateMarineBTv100();
			//GenerateBTv010();
		}
		else if(unit.unitType == "sniper"){
			GenerateSniperBTv100();
		}
		else if(unit.unitType == "explosives"){
			GenerateExplosivesBTv100();
		}
	}

	void GenerateExplosivesBTv100(){
		//temporary generic bt
		GenerateBTv010();
	}

	void GenerateSniperBTv100(){
		BTNode f0 = new FallbackNode(unit);

				BTNode s0 = new SequenceNode(unit);
				f0.children.Add(s0);

						BTNode c0 = new IsCommandIdle(unit);
						s0.children.Add(c0);

						BTNode s1 = new SequenceNode(unit);
						s0.children.Add(s1);

								BTNode c1 = new BeingShotAt(unit);
								s1.children.Add(c1);

								BTNode a0 = new RunToTheHills(unit);
								s1.children.Add(a0);

				BTNode s2 = new SequenceNode(unit);
				f0.children.Add(s2);

						BTNode c2 = new IsCommandMove(unit);
						s2.children.Add(c2);

						BTNode f1 = new FallbackNode(unit);
						s2.children.Add(f1);

								BTNode s3 = new SequenceNode(unit);
								f1.children.Add(s3);

										BTNode c3 = new BeingShotAt(unit);
										s3.children.Add(c3);

										BTNode sf0 = new CSF0(unit);
										s3.children.Add(sf0);

											BTNode a1 = new RunToTheHills(unit);
											sf0.children.Add(a1);

											BTNode f2 = new FallbackNode(unit);
											sf0.children.Add(f2);

													BTNode a2 = new TakeCover(unit, emptySlot);
													f2.children.Add(a2);

													BTNode a3 = new RunToTheHills(unit);
													f2.children.Add(a3);

								BTNode s4 = new SequenceNode(unit);
								f1.children.Add(s4);

										BTNode c4 = new NotAtTargetLocation(unit);
										s4.children.Add(c4);

										BTNode a4 = new GoTowardsTargetLocation(unit);
										s4.children.Add(a4);

								BTNode a5 = new MakeUnitIdle(unit);
								f1.children.Add(a5);

		root = f0;
	}

	void GenerateMarineBTv100(){
		//temporary generic bt
		GenerateBTv010();
	}

	void GenerateSpotterBTv100(){
		BTNode f0 = new FallbackNode(unit);

				BTNode s0 = new SequenceNode(unit);
				f0.children.Add(s0);

						BTNode c0 = new IsCommandIdle(unit);
						s0.children.Add(c0);

						BTNode s1 = new SequenceNode(unit);
						s0.children.Add(s1);

								BTNode c1 = new BeingShotAt(unit);
								s1.children.Add(c1);

								BTNode f1 = new FallbackNode(unit);
								s1.children.Add(f1);

										BTNode a0 = new TakeCover(unit, emptySlot);
										f1.children.Add(a0);

										BTNode a1 = new RunToTheHills(unit);
										f1.children.Add(a1);

				BTNode s2 = new SequenceNode(unit);
				f0.children.Add(s2);

						BTNode c2 = new IsCommandMove(unit);
						s2.children.Add(c2);

						BTNode f2 = new FallbackNode(unit);
						s2.children.Add(f2);

								BTNode s3 = new SequenceNode(unit);
								f2.children.Add(s3);

										BTNode c3 = new BeingShotAt(unit);
										s3.children.Add(c3);

										BTNode f3 = new FallbackNode(unit);
										s3.children.Add(f3);

												BTNode s4 = new SequenceNode(unit);
												f3.children.Add(s4);

														BTNode c4 = new IsEnemyWeakerThanUs(unit);
														s4.children.Add(c4);

														BTNode f4 = new FallbackNode(unit);
														s4.children.Add(f4);

																BTNode a2 = new TakeCover(unit, emptySlot);
																f4.children.Add(a2);

																BTNode a3 = new RunToTheHills(unit);
																f4.children.Add(a3);

												BTNode a4 = new RunToTheHills(unit);
												f3.children.Add(a4);

								BTNode s5 = new SequenceNode(unit);
								f2.children.Add(s5);

										BTNode c5 = new NotAtTargetLocation(unit);
										s5.children.Add(c5);

										BTNode a5 = new GoTowardsTargetLocation(unit);
										s5.children.Add(a5);

								BTNode a6 = new MakeUnitIdle(unit);
								s2.children.Add(a6);

		root = f0;
	}

	void GenerateBTv010(){
		BTNode f0 = new FallbackNode(unit);

				BTNode s0 = new SequenceNode(unit);
				f0.children.Add(s0);

						BTNode c0 = new IsCommandIdle(unit);
						s0.children.Add(c0);

						BTNode s1 = new SequenceNode(unit);
						s0.children.Add(s1);

								BTNode c1 = new BeingShotAt(unit);
								s1.children.Add(c1);

								BTNode f1 = new FallbackNode(unit);
								s1.children.Add(f1);

										BTNode a0 = new TakeCover(unit, emptySlot);
										f1.children.Add(a0);

										BTNode a1 = new RunToTheHills(unit);
										f1.children.Add(a1);

				BTNode s2 = new SequenceNode(unit);
				f0.children.Add(s2);

						BTNode c2 = new IsCommandHoldPosition(unit);
						s2.children.Add(c2);

						BTNode f2 = new FallbackNode(unit);
						s2.children.Add(f2);

								BTNode s3 = new SequenceNode(unit);
								f2.children.Add(s3);

										BTNode c3 = new IsEnemyNearby(unit);
										s3.children.Add(c3);

										BTNode f3 = new FallbackNode(unit);
										s3.children.Add(f3);

												BTNode a2 = new TakeCover(unit, emptySlot);
												f3.children.Add(a2);

												BTNode a3 = new PushForward(unit);
												f3.children.Add(a3);

								BTNode f4 = new FallbackNode(unit);
								f2.children.Add(f4);

										BTNode s4 = new SequenceNode(unit);
										f4.children.Add(s4);

												BTNode c4 = new AwayFromTargetLocation(unit);
												s4.children.Add(c4);

												BTNode a4 = new GoTowardsTargetLocation(unit);
												s4.children.Add(a4);

										BTNode s10 = new SequenceNode(unit);
										f4.children.Add(s10);

												BTNode a5 = new Diffuse(unit, emptySlot);
												s10.children.Add(a5);

												BTNode a10 = new MakeUnitInPosition(unit);
												s10.children.Add(a10);

				BTNode s9 = new SequenceNode(unit);
				f0.children.Add(s9);

						BTNode c9 = new IsCommandInPosition(unit);
						s9.children.Add(c9);

				BTNode s5 = new SequenceNode(unit);
				f0.children.Add(s5);

						BTNode c5 = new IsCommandMove(unit);
						s5.children.Add(c5);

						BTNode f5 = new FallbackNode(unit);
						s5.children.Add(f5);

								BTNode s6 = new SequenceNode(unit);
								f5.children.Add(s6);

										BTNode c6 = new BeingShotAt(unit);
										s6.children.Add(c6);

										BTNode f6 = new FallbackNode(unit);
										s6.children.Add(f6);
												
												BTNode s7 = new SequenceNode(unit);
												f6.children.Add(s7);

														BTNode c7 = new IsEnemyWeakerThanUs(unit);
														s7.children.Add(c7);

														BTNode a6 = new PushForward(unit);
														s7.children.Add(a6);
												
												BTNode a7 = new RunToTheHills(unit);
												f6.children.Add(a7);

								BTNode s8 = new SequenceNode(unit);
								f5.children.Add(s8);

										BTNode c8 = new NotAtTargetLocation(unit);
										s8.children.Add(c8);

										BTNode a8 = new GoTowardsTargetLocation(unit);
										s8.children.Add(a8);

								BTNode a9 = new MakeUnitIdle(unit);
								f5.children.Add(a9);

		root = f0;
	}
	
}

namespace Belin.Lcov;

[TestClass]
public sealed class BranchCoverageTest {

	[TestMethod]
	public void TestToString() {
		var data = new BranchData { BlockNumber = 3, BranchNumber = 2, LineNumber = 127, Taken = 1 };
		AreEqual("BRF:0\nBRH:0", new BranchCoverage().ToString());
		AreEqual($"{data}\nBRF:23\nBRH:11", new BranchCoverage { Data = [data], Found = 23, Hit = 11 }.ToString());
	}
}

[TestClass]
public sealed class BranchDataTest {

	[TestMethod]
	public void TestToString() {
		AreEqual("BRDA:0,0,0,-", new BranchData().ToString());
		AreEqual("BRDA:127,3,2,1", new BranchData { BlockNumber = 3, BranchNumber = 2, LineNumber = 127, Taken = 1 }.ToString());
	}
}

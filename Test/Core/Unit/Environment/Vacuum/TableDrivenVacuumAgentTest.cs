namespace aima.test.core.unit.environment.vacuum;

using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

using AIMA.Core.Environment.Vacuum.TableDrivenVacuumAgent;
using AIMA.Core.Environment.Vacuum.VacuumEnvironment;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class TableDrivenVacuumAgentTest {
	private TableDrivenVacuumAgent agent;

	private StringBuilder envChanges;

	@Before
	public void setUp() {
		agent = new TableDrivenVacuumAgent();
		envChanges = new StringBuilder();
	}

	@Test
	public void testCleanClean() {
		VacuumEnvironment tve = new VacuumEnvironment(
				VacuumEnvironment.LocationState.Clean,
				VacuumEnvironment.LocationState.Clean);
		tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

		tve.addEnvironmentView(new EnvironmentViewActionTracker(envChanges));

		tve.stepUntilDone();

		Assert
				.assertEquals(
						"Action[name==Right]Action[name==Left]Action[name==Right]Action[name==NoOp]",
						envChanges.ToString());
	}

	@Test
	public void testCleanDirty() {
		VacuumEnvironment tve = new VacuumEnvironment(
				VacuumEnvironment.LocationState.Clean,
				VacuumEnvironment.LocationState.Dirty);
		tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

		tve.addEnvironmentView(new EnvironmentViewActionTracker(envChanges));

		tve.stepUntilDone();

		Assert
				.assertEquals(
						"Action[name==Right]Action[name==Suck]Action[name==Left]Action[name==NoOp]",
						envChanges.ToString());
	}

	@Test
	public void testDirtyClean() {
		VacuumEnvironment tve = new VacuumEnvironment(
				VacuumEnvironment.LocationState.Dirty,
				VacuumEnvironment.LocationState.Clean);
		tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

		tve.addEnvironmentView(new EnvironmentViewActionTracker(envChanges));

		tve.stepUntilDone();

		Assert
				.assertEquals(
						"Action[name==Suck]Action[name==Right]Action[name==Left]Action[name==NoOp]",
						envChanges.ToString());
	}

	@Test
	public void testDirtyDirty() {
		VacuumEnvironment tve = new VacuumEnvironment(
				VacuumEnvironment.LocationState.Dirty,
				VacuumEnvironment.LocationState.Dirty);
		tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

		tve.addEnvironmentView(new EnvironmentViewActionTracker(envChanges));

		tve.stepUntilDone();

		Assert
				.assertEquals(
						"Action[name==Suck]Action[name==Right]Action[name==Suck]Action[name==NoOp]",
						envChanges.ToString());
	}
}

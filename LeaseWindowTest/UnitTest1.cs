using System;
using System.Linq;
using NUnit.Framework;
using LLMS.ViewModel;

namespace LLMS.Tests
{
    [TestFixture]
    public class LeaseWindowViewModelTests
    {
        [Test]
        public void CanAddLease()
        {
            // Arrange
            var viewModel = new LeaseWindowViewModel();
            var initialLeaseCount = viewModel.Leases.Count;

            // Act
            viewModel.AddCommand.Execute(null); // Passing null as parameter

            // Assert
            Assert.AreEqual(initialLeaseCount + 1, viewModel.Leases.Count);
            var newlyAddedLease = viewModel.Leases.Last();
            Assert.IsNotNull(newlyAddedLease);
            // Add more assertions if needed to validate the properties of the newly added lease
        }

        [Test]
        public void CanUpdateLease()
        {
            // Arrange
            var viewModel = new LeaseWindowViewModel();
            // Assuming there is at least one lease initially
            var leaseToUpdate = viewModel.Leases.FirstOrDefault();
            if (leaseToUpdate == null)
            {
                Assert.Inconclusive("No lease available to update.");
            }

            var originalLeaseClauses = leaseToUpdate.lease_clauses;
            var updatedLeaseClauses = "Updated lease clauses";

            // Act
            viewModel.SelectedLease = leaseToUpdate;
            viewModel.LeaseClauses = updatedLeaseClauses;
            viewModel.UpdateCommand.Execute(null); // Passing null as parameter

            // Assert
            var updatedLease = viewModel.Leases.FirstOrDefault(l => l.id == leaseToUpdate.id);
            Assert.IsNotNull(updatedLease);
            Assert.AreEqual(updatedLeaseClauses, updatedLease.lease_clauses);
            // Add more assertions if needed to validate other properties
        }

        [Test]
        public void CanDeleteLease()
        {
            // Arrange
            var viewModel = new LeaseWindowViewModel();
            // Assuming there is at least one lease initially
            var leaseToDelete = viewModel.Leases.FirstOrDefault();
            if (leaseToDelete == null)
            {
                Assert.Inconclusive("No lease available to delete.");
            }

            var initialLeaseCount = viewModel.Leases.Count;

            // Act
            viewModel.SelectedLease = leaseToDelete;
            viewModel.DeleteCommand.Execute(null); // Passing null as parameter

            // Assert
            Assert.AreEqual(initialLeaseCount - 1, viewModel.Leases.Count);
            Assert.IsFalse(viewModel.Leases.Contains(leaseToDelete));
        }
    }
}
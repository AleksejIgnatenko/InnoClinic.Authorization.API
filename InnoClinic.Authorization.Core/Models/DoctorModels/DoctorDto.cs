namespace InnoClinic.Authorization.Core.Models.DoctorModels
{
    /// <summary>
    /// Represents a data transfer object for a doctor.
    /// </summary>
    public class DoctorDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the doctor.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated account.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the doctor.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the doctor.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the middle name of the doctor.
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cabinet number of the doctor.
        /// </summary>
        public int CabinetNumber { get; set; }

        /// <summary>
        /// Gets or sets the status of the doctor (e.g., active, inactive).
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}

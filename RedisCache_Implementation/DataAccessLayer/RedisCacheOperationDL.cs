using Microsoft.AspNetCore.Mvc;
using RedisCache_Implementation.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace RedisCache_Implementation.DataAccessLayer
{
    public class RedisCacheOperationDL : IRedisCacheOperationDL
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _connection;


        public RedisCacheOperationDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }





        public async Task<AddInformationResponse> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse();
            response.IsSuccess = true;
            response.Message = "Information added successfully.";

            try
            {
                // Create a new SqlConnection instance using the connection string
                    if (_connection.State != ConnectionState.Open)
                        await _connection.OpenAsync();

                    string query = "INSERT INTO USERINFO (UserName, EmailID, MobileNumber, Salary, Gender) VALUES (@UserName, @EmailID, @MobileNumber, @Salary, @Gender)";

                    using (SqlCommand command = new SqlCommand(query, _connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 30;
                        command.Parameters.AddWithValue("@UserName", request.UserName);
                        command.Parameters.AddWithValue("@EmailID", request.EmailID);
                        command.Parameters.AddWithValue("@MobileNumber", request.MobileNumber);
                        command.Parameters.AddWithValue("@Salary", request.Salary);
                        command.Parameters.AddWithValue("@Gender", request.Gender);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected <= 0)
                        {
                            response.IsSuccess = false;
                            response.Message = "Failed to add information.";
                        }
                    }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {ex.Message}";
            }
            finally
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }

            return response;
        }

        public Task<DeleteInformationResponse> DeleteInformation(DeleteInformationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<GetInformationResponse> GetInformation()
        {
            GetInformationResponse response = new GetInformationResponse();
            response.IsSuccess = true;
            response.Message = "Information retrieved successfully.";

            try
            {
                if(_connection.State != ConnectionState.Open)
                    await _connection.OpenAsync();

                string query = "SELECT * FROM USERINFO";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 30;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            response.data = new List<GetInformation>();

                            while(await reader.ReadAsync())
                            {
                                GetInformation getData = new GetInformation();
                                getData.UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty;
                                getData.EmailID = reader["EmailID"] != DBNull.Value ? reader["EmailID"].ToString() : string.Empty;
                                getData.MobileNumber = reader["MobileNumber"] != DBNull.Value ? reader["MobileNumber"].ToString() : String.Empty;
                                getData.Salary = reader["Salary"] != DBNull.Value ? Convert.ToInt32(reader["Salary"]) : -1;
                                getData.Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : String.Empty;

                                response.data.Add(getData);

                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }

            return response;
        }

        public Task<RefreshRecordTimeResponse> RefreshRecordTime()
        {
            throw new NotImplementedException();
        }

        public Task<UpdateInformationResponse> UpdateInformation(UpdateInformationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

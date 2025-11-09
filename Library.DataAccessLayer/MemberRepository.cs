using System;
using System.Data;

using Library.Entities;
using Library.DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;

namespace Library.DataAccessLayer
{
    public class MemberRepository : IMemberRepository
    {
        public int AddMember(Member member)
        {
            int MemberID = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Members (FullName, Email, Phone, JoinDate, IsActive)
                                 VALUES (@FullName, @Email, @Phone, @JoinDate, @IsActive);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", member.FullName);
                    command.Parameters.AddWithValue("@Email", member.Email);
                    command.Parameters.AddWithValue("@Phone", member.Phone);
                    command.Parameters.AddWithValue("@JoinDate", member.JoinDate);
                    command.Parameters.AddWithValue("@IsActive", member.IsActive);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        MemberID = insertedID;
                }
            }
            return MemberID;
        }

        public Member GetMemberById(int memberId)
        {
            Member member = null;
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = @"SELECT MemberID, FullName, Email, Phone, JoinDate, IsActive 
                                 FROM Members 
                                 WHERE MemberID = @MemberID AND IsActive = 1;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MemberID", memberId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        member = new Member
                        {
                            MemberID = (int)reader["MemberID"],
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            JoinDate = Convert.ToDateTime(reader["JoinDate"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                    }
                }
            }
            return member;
        }

        public DataTable GetAllMembers()
        {
            DataTable allMembersTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT MemberID, FullName, Email, Phone, JoinDate, IsActive FROM Members;", connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        allMembersTable.Load(reader);
                }
            }
            return allMembersTable;
        }

        public bool UpdateMember(Member member)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Members 
                                 SET FullName = @FullName, 
                                     Email = @Email, 
                                     Phone = @Phone, 
                                     JoinDate = @JoinDate, 
                                     IsActive = @IsActive 
                                 WHERE MemberID = @MemberID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", member.FullName);
                    command.Parameters.AddWithValue("@Email", member.Email);
                    command.Parameters.AddWithValue("@Phone", member.Phone);
                    command.Parameters.AddWithValue("@JoinDate", member.JoinDate);
                    command.Parameters.AddWithValue("@IsActive", member.IsActive);
                    command.Parameters.AddWithValue("@MemberID", member.MemberID);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }

        public bool DeleteMember(int id)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Members WHERE MemberID = @MemberID;", connection))
            {
                command.Parameters.AddWithValue("@MemberID", id);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }
    }
}

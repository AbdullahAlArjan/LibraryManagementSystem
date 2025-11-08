using System;

namespace Library.DataAccessLayer.Interfaces;

public interface IMemberRepository
{
    void AddMember(Member member);
    Member GetMemberById(int id);
    DataTable GetAllMembers();
    void UpdateMember(Member member);
    void DeleteMember(int id);
    
}

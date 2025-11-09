using System.Data;
using Library.Entities;

namespace Library.DataAccessLayer.Interfaces
{
    public interface IMemberRepository
    {
        int AddMember(Member member);
        Member GetMemberById(int memberId);   
        DataTable GetAllMembers();
        bool UpdateMember(Member member);
        bool DeleteMember(int id);
    }
}

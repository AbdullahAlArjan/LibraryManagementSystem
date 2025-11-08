using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DataAccessLayer.Interfaces;
using Library.DataAccessLayer;

namespace Library.BusinessLayer
{
    public class MemberManager : IMemberRepository
    {
        private readonly IMemberRepository _memberRepository;

        public MemberManager(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public void AddMember(Member member)
        {
            _memberRepository.AddMember(member);
        }
        public Member GetMemberById(int id)
        {
            return _memberRepository.GetMemberById(id);
        }
        public DataTable GetAllMembers()
        {
            return _memberRepository.GetAllMembers();
        }
        public void UpdateMember(Member member)
        {
            _memberRepository.UpdateMember(member);
        }
        public void DeleteMember(int id)
        {
            _memberRepository.DeleteMember(id);
        }

    }
}

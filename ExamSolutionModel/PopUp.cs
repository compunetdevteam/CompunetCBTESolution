namespace ExamSolutionModel
{
    public enum Salutation
    {
        Dr = 1, Nurse, Mr, Mrs, Miss, Engr, Pastor
    }
    public enum Relationship
    {
        Father = 1, Mother, Sister, Brother, Friend, Others
    }
    public enum Gender
    {
        Male = 1, Female
    }

 
    public enum HealthStatus
    {
        Excellent, Good, Poor
    }

    public enum ResultTypeGrade
    {
        A1 = 1, B2, B3, C4, C5, C6, D7, E8, F9
    }
 
    public enum Qualifications
    {
        SSCE,
        NCE,
        OND,
        HND,
        Degree,
        Masters
    }

    public enum ThemeColor
    {
        Purple = 1, LightBlue, NavyBlue, ArmyGreen, LightRed, DeepRed
    }


    public enum State
    {
        Abia, Adamawa, AkwaIbom, Anambra, Bauchi, Bayelsa, Benue, Borno, CrossRiver, Delta, Ebonyi, Edo, Ekiti,
        Abuja, Gombe, Imo, Jigawa, Kaduna, Kano, Katsina, Kebbi, Kogi, Kwara, Lagos, Nasarawa, Niger, Ogun, Ondo, Osun,
        Oyo, Plateau, Rivers, Sokoto, Taraba, Yobe, Zamfara
    }
    public enum QuestionType
    {

        SingleChoice, MultiChoice, BlankChoice
    }

    public enum ResultType
    {

        WAEC, NECO, GCE
    }

    public enum ProgrammeCategory
    {
        UnderGraduate = 1, PostGraduate
    }

    public enum ProgrammeType
    {
        Full_Time = 1, Part_Time
    }

    public enum StudentSms
    {
        All_Student = 1, Registered_Student, All_Staff
    }
    public enum StaffRole
    {
        Lecturer = 1, None_Teaching
    }


}

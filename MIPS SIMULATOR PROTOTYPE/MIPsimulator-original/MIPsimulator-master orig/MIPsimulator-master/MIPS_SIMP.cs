using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MIPS
{
    public partial class MIPS_SIMP : Form
    {
        byte[] registerFile = new byte[32];
        List<byte> data_memory = new List<byte>();
        List<int> instruction_memory = new List<int>();
        Execution call_to_execution = new Execution();
        string instruction;
        private int programe_counter = 0; /*error_counter = 0;*/

        public MIPS_SIMP()
        {
            InitializeComponent();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();            
        }
        private void reset_reg_file()
        {
            for (int i = 0; i < registerFile.Length; i++)
            {
                registerFile[i] = 0;
            }
            update_regFile();
        }
        private void resetRegFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset_reg_file();
        }
        private void toolStripButton_run_Click(object sender, EventArgs e)
        {
            instruction_memory.Clear();
            reset_reg_file();
            call_to_execution.dataMem = this.data_memory;
            
            code_building();
            
            if (call_to_execution.erros > 0)
            {
                MessageBox.Show("error found","",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            compile_code();            
            
            registerFile = call_to_execution.registerFile;
            update_regFile();
            update_data_memory();
        }

        private void update_data_memory()
        {
            data_memory = call_to_execution.dataMem;
            richTextBox_dataMem.Clear();
            byte[] temp_data_mem;
            temp_data_mem = data_memory.ToArray();
            for (int i = 0; i < temp_data_mem.Length; i++)
            {
                string tmp = Convert.ToString(data_memory[i], 16).PadLeft(8,'0');
                tmp += '\n';
                richTextBox_dataMem.AppendText(tmp);
            }
        }

        private void update_regFile()
        {
            label_r00.Text = Convert.ToString(registerFile[0], 10).PadLeft(8, '0');
            label_r1.Text = Convert.ToString(registerFile[1], 10).PadLeft(8, '0');
            label_r2.Text = Convert.ToString(registerFile[2], 10).PadLeft(8, '0');
            label_r3.Text = Convert.ToString(registerFile[3], 10).PadLeft(8, '0');
            label_r4.Text = Convert.ToString(registerFile[4], 10).PadLeft(8, '0');
            label_r5.Text = Convert.ToString(registerFile[5], 10).PadLeft(8, '0');
            label_r6.Text = Convert.ToString(registerFile[6], 10).PadLeft(8, '0');
            label_r7.Text = Convert.ToString(registerFile[7], 10).PadLeft(8, '0');
            label_r8.Text = Convert.ToString(registerFile[8], 10).PadLeft(8, '0');
            label_r9.Text = Convert.ToString(registerFile[9], 10).PadLeft(8, '0');
            label_r10.Text = Convert.ToString(registerFile[10], 10).PadLeft(8, '0');
            label_r11.Text = Convert.ToString(registerFile[11], 10).PadLeft(8, '0');
            label_r12.Text = Convert.ToString(registerFile[12], 10).PadLeft(8, '0');
            label_r13.Text = Convert.ToString(registerFile[13], 10).PadLeft(8, '0');
            label_r14.Text = Convert.ToString(registerFile[14], 10).PadLeft(8, '0');
            label_r15.Text = Convert.ToString(registerFile[15], 10).PadLeft(8, '0');
            label_r016.Text = Convert.ToString(registerFile[16], 10).PadLeft(8, '0');
            label_r17.Text = Convert.ToString(registerFile[17], 10).PadLeft(8, '0');
            label_r18.Text = Convert.ToString(registerFile[18], 10).PadLeft(8, '0');
            label_r19.Text = Convert.ToString(registerFile[19], 10).PadLeft(8, '0');
            label_r20.Text = Convert.ToString(registerFile[20], 10).PadLeft(8, '0');
            label_r21.Text = Convert.ToString(registerFile[21], 10).PadLeft(8, '0');
            label_r22.Text = Convert.ToString(registerFile[22], 10).PadLeft(8, '0');
            label_r23.Text = Convert.ToString(registerFile[23], 10).PadLeft(8, '0');
            label_r24.Text = Convert.ToString(registerFile[24], 10).PadLeft(8, '0');
            label_r25.Text = Convert.ToString(registerFile[25], 10).PadLeft(8, '0');
            label_r26.Text = Convert.ToString(registerFile[26], 10).PadLeft(8, '0');
            label_r27.Text = Convert.ToString(registerFile[27], 10).PadLeft(8, '0');
            label_r28.Text = Convert.ToString(registerFile[28], 10).PadLeft(8, '0');
            label_r29.Text = Convert.ToString(registerFile[29], 10).PadLeft(8, '0');
            label_r30.Text = Convert.ToString(registerFile[30], 10).PadLeft(8, '0');
            label_r31.Text = Convert.ToString(registerFile[31], 10).PadLeft(8, '0');
        }

        private void code_building()
        {
            int counter = 0, pc =0;            
            while (counter < richTextBox_codeArena.Lines.Length)
            {
                instruction = richTextBox_codeArena.Lines[counter];
                if (instruction.Length == 0)
                {
                    counter++;
                    continue;
                }
                if (instruction[0] == '#' || instruction[0] == '\n')
                {
                    counter++;
                    continue;
                }
                call_to_execution.code_into_MIPS(instruction);
                registerFile = call_to_execution.registerFile;
                instruction_memory = call_to_execution.instructionMemory;                
                richTextBox_instMem.AppendText(Convert.ToString(instruction_memory[pc], 2).PadLeft(32,'0')  + '\n');
                call_to_execution.machineInstruction = "";
                counter++;
                pc++;                
            }
            data_memory = call_to_execution.dataMem;            
        }

        private void compile_code()
        {
            programe_counter = 0;
            call_to_execution.registerFile = registerFile;
            string instruction = "";
            int[] tmp = instruction_memory.ToArray();
            while (programe_counter < tmp.Length)
            {
                instruction = richTextBox_instMem.Lines[programe_counter];
                call_to_execution.CompileMachineInst(instruction);
                programe_counter++;
            }           
        }

        private void dataMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data_mem_filePath = load_file();
            richTextBox_dataMem.Text = File.ReadAllText(data_mem_filePath);
            string[] hexValuesSplit = richTextBox_dataMem.Text.Split('\n', '\r');
            //adding the data memory to the list:
            for (int i = 0; i < hexValuesSplit.Length; i+=2)
            {
                int value = Convert.ToInt32(hexValuesSplit[i], 16);
                data_memory.Add((byte)value);
            }
            call_to_execution.dataMem = data_memory;
        }       
        private string load_file()
        {
            string filePath = "";
            OpenFileDialog file_dialog = new OpenFileDialog();
            file_dialog.Filter = "Desktop|*.txt";
            System.Windows.Forms.DialogResult dialog_result = file_dialog.ShowDialog();
            if (dialog_result == DialogResult.OK)
            {
                filePath = file_dialog.FileName;
            }
            return filePath;
        }

        private void toolStripButton_emptyInstMemTxt_Click(object sender, EventArgs e)
        {
            //data_memory.Clear();
            instruction_memory.Clear();
            richTextBox_instMem.Clear();
            call_to_execution.machineInstruction = "";
            call_to_execution.instructionMemory.Clear();
            instruction_memory.Clear();
            reset_reg_file();
        }

        private void hexFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}





class Execution
{

    public string[] store = new string[4];
    public string[] store2 = new string[6];
    private byte[] register_file = new byte[32];
    private List<byte> dataMemory = new List<byte>();
    private List<int> inst_mem = new List<int>();
    private int opCode = 0, offset = 0, rs = 0, immediate = 0, rt = 0, rd = 0, shamt = 0, function = 0;
    private int target_address = 0;
    private bool Rtype = false, Itype = false, Jtype = false, branch = false;
    private string machine_instruction = "";
    private int pc = 0;
    private int error_counter = 0;
    public Execution()
    {

    }

    public void code_into_MIPS(string instruction)
    {
        char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n', '$', '(', ')', 't' ,'s'};
        string[] words = instruction.Split(delimiterChars);
        if (words[0] == "slt" || words[0] == "add" || words[0] == "sub" ||
            words[0] == "or" || words[0] == "and" || words[0] == "xor")
        {
            opCode = 0;
            shamt = 0;
            if (int.TryParse(words[3], out rd)) { }
            if (int.TryParse(words[6], out rs)) { }
            if (int.TryParse(words[9], out rt)) { }
        }

        switch (words[0])
        {
            case "nop":
                opCode = 0;
                rd = rs = rt = function = shamt = 0;
                Rtype = true;
                get_machine_instruction();
                break;
            case "add":
                function = Convert.ToInt32("100000", 2);
                Rtype = true;
                get_machine_instruction();
                break;
            case "sub":
                function = Convert.ToInt32("100010", 2);
                Rtype = true;
                get_machine_instruction();
                break;
            case "or":
                function = Convert.ToInt32("100101", 2);
                Rtype = true;
                get_machine_instruction();
                break;
            case "and":
                function = Convert.ToInt32("100100", 2);
                Rtype = true;
                get_machine_instruction();
                break;
            case "xor":
                function = Convert.ToInt32("100110", 2);
                Rtype = true;
                get_machine_instruction();
                break;
            case "slt":
                function = Convert.ToInt32("101010", 2);
                Rtype = true;
                get_machine_instruction();
                break;
            case "sll":
                function = Convert.ToInt32("000000", 2);
                if (int.TryParse(words[2], out rt)) { }
                if (int.TryParse(words[3], out shamt)) { }
                Rtype = true;
                get_machine_instruction();
                break;
            case "srl":
                function = Convert.ToInt32("000010", 2);
                if (int.TryParse(words[2], out rt)) { }
                if (int.TryParse(words[3], out shamt)) { }
                Rtype = true;
                get_machine_instruction();
                break;
            case "jr":
                function = Convert.ToInt32("001000", 2);
                if (int.TryParse(words[3], out rs)) { }
                Rtype = true;
                get_machine_instruction();
                break;
            case "addi":
                opCode = 8;
                if (int.TryParse(words[3], out rt)) { }
                if (int.TryParse(words[6], out rs)) { }
                if (int.TryParse(words[7], out immediate)) { }
                Itype = true;
                get_machine_instruction();
                break;
            case "lw":
                if (dataMemory == null)
                    error_counter++;
                opCode = 35;
                if (int.TryParse(words[7], out rs)) { }
                if (int.TryParse(words[4], out immediate)) { }
                if (int.TryParse(words[3], out rt)) { }
                offset = immediate + register_file[rs];
                Itype = true;
                get_machine_instruction();
                break;
            case "sw":
                if (dataMemory == null)
                    error_counter++;
                opCode = 43;
                if (int.TryParse(words[7], out rs)) { }
                if (int.TryParse(words[4], out immediate)) { }
                if (int.TryParse(words[3], out rt)) { }
                offset = immediate + register_file[rs];
                Itype = true;
                get_machine_instruction();
                break;
            case "j":
                opCode = 2;
                if (int.TryParse(words[1], out target_address)) { }
                Jtype = true;
                get_machine_instruction();
                break;
            case "beq":
                opCode = 4;
                if (int.TryParse(words[2], out rt)) { }
                if (int.TryParse(words[4], out rs)) { }
                if (int.TryParse(words[5], out immediate)) { }
                branch = true;
                get_machine_instruction();
                break;
            case "bnq":
                opCode = 5;
                if (int.TryParse(words[2], out rt)) { }
                if (int.TryParse(words[4], out rs)) { }
                if (int.TryParse(words[5], out immediate)) { }
                branch = true;
                get_machine_instruction();
                break;
        }
    }

    private void get_machine_instruction()
    {
        machine_instruction += Convert.ToString(opCode, 2).PadLeft(6, '0');
        store[0] = machine_instruction;
        store2[0] = machine_instruction;
        if (Rtype)
        {
            machine_instruction += Convert.ToString(rs, 2).PadLeft(5, '0');
            store2[1] = Convert.ToString(rs, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(rt, 2).PadLeft(5, '0');
            store2[2] = Convert.ToString(rt, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(rd, 2).PadLeft(5, '0');
            store2[3] = Convert.ToString(rd, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(shamt, 2).PadLeft(5, '0');
            store2[4] = Convert.ToString(shamt, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(function, 2).PadLeft(6, '0');
            store2[5] = Convert.ToString(function, 2).PadLeft(6, '0');
            MessageBox.Show("\tR TYPE INSTRUCTION\nOPCODE: " + store2[0] + "\n" + "SOURCE REGISTER1 : " + store2[1] + "\n" + "SOURCE REGISTER2: " + store2[2] + "\n" + "DESTENATION REGISTER: " + store2[3] + "\n" + "SHIFT AMOUNT: " + store2[4] + "\n" + "FUNCTION: " + store2[5] + "\n");
        }
        else if (Itype)
        {
            machine_instruction += Convert.ToString(rs, 2).PadLeft(5, '0');
            store[1] = Convert.ToString(rs, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(rt, 2).PadLeft(5, '0');
            store[2] = Convert.ToString(rd, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(immediate, 2).PadLeft(16, '0');
            store[3] = Convert.ToString(immediate, 2).PadLeft(16, '0');
            MessageBox.Show("\tI TYPE INSTRUCTION\nOPCODE: "+store[0] + "\n" +"SOURCE REGISTER: " +store[1] + "\n"+"DESTENATION REGISTER: "+store[2] + "\n"+"IMMEDIATE: "+store[3] + "\n");
        }
        else if (Jtype)
        {
            machine_instruction += Convert.ToString(target_address, 2).PadLeft(26, '0');
        }
        else if (branch)
        {
            machine_instruction += Convert.ToString(rs, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(rt, 2).PadLeft(5, '0');
            machine_instruction += Convert.ToString(offset, 2).PadLeft(16, '0');
        }
        inst_mem.Add((int)(Convert.ToInt64(machine_instruction, 2)));

        machine_instruction += '\n';

        Rtype = false;
        Itype = false;
        Jtype = false;
        branch = false;
    }
    public void CompileMachineInst(string inst)
    {
        pc = 0;
        opCode = (int)Convert.ToInt64(inst.Substring(0, 6), 2);
        if (opCode == 8 || opCode == 35 || opCode == 43 || opCode == 4 || opCode == 5)
        {
            rs = (int)Convert.ToInt64(inst.Substring(6, 5), 2);
            rt = (int)Convert.ToInt64(inst.Substring(11, 5), 2);
            immediate = (int)Convert.ToInt64(inst.Substring(16, 16), 2);
        }
        if (opCode != 2 && opCode != 3 && opCode != 4 && opCode != 5)
        {
            pc++;
        }
        switch (opCode)
        {
            case 0:
                #region Rtype
                rs = (int)Convert.ToInt64(inst.Substring(6, 5), 2);
                rt = (int)Convert.ToInt64(inst.Substring(11, 5), 2);
                rd = (int)Convert.ToInt64(inst.Substring(16, 5), 2);
                shamt = (int)Convert.ToInt64(inst.Substring(21, 6), 2);
                function = (int)Convert.ToInt64(inst.Substring(26, 6), 2);

                switch (function)
                {
                    case 42: //101010 :slt
                        if (register_file[rs] < register_file[rt])
                            register_file[rd] = 1;
                        else
                            register_file[rd] = 0;
                        break;
                    case 32: //100000 :add
                        register_file[rd] = (Byte)(register_file[rs] + register_file[rt]);
                        break;
                    case 34: //100010 :sub
                        register_file[rd] = (Byte)(register_file[rs] - register_file[rt]);
                        break;
                    case 37: //100101 :or
                        register_file[rd] = (Byte)(register_file[rs] | register_file[rt]);
                        break;
                    case 36: //100100 :and
                        register_file[rd] = (Byte)(register_file[rs] & register_file[rt]);
                        break;
                    case 38: //100110 :xor
                        register_file[rd] = (Byte)(register_file[rs] ^ register_file[rt]);
                        break;
                    case 00: //000000 :sll
                        register_file[rd] = (Byte)(register_file[rt] * Math.Pow(2, shamt));
                        break;
                    case 02: //000010 :srl
                        register_file[rd] = (Byte)(register_file[rt] * Math.Pow(2, -shamt));
                        break;
                }
                #endregion //Rtype                    
                break;
            case 8: //addi                    
                register_file[rt] = (Byte)(register_file[rs] + immediate);
                break;
            case 35: //lw                    
                offset = immediate + register_file[rs];
                register_file[rt] = dataMemory[offset];
                break;
            case 43: //sw
                offset = immediate + register_file[rs];
                dataMemory[offset] = register_file[rt];
                break;
            case 2: //jump                    
                pc = (int)Convert.ToInt64(inst.Substring(6, 26), 2);
                break;
            case 4: //beq
                if (register_file[rt] == register_file[rs])
                    pc += immediate;
                else
                    pc++;
                break;
            case 5: //bnq
                if (register_file[rt] != register_file[rs])
                    pc += immediate;
                else
                    pc++;
                break;
        }
    }

    public int erros
    {
        get { return this.error_counter; }
        set { this.error_counter = value; }
    }
    public byte[] registerFile
    {
        get { return this.register_file; }
        set { this.register_file = value; }
    }
    public List<byte> dataMem
    {
        get { return this.dataMemory; }
        set { this.dataMemory = value; }
    }
    public List<int> instructionMemory
    {
        get { return this.inst_mem; }
        set { this.inst_mem = value; }
    }
    public string machineInstruction
    {
        get { return this.machine_instruction; }
        set { this.machine_instruction = value; }
    }
}


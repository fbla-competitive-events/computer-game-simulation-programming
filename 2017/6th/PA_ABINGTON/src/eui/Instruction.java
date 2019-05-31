package eui;

public class Instruction {

	private int id;
	private String name;
	private Runnable code;
	
	public Instruction(int id, String name, Runnable code){ // TODO 
		this.setId(id);
		this.setName(name);
		this.setCode(code);
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Runnable getCode() {
		return code;
	}

	public void setCode(Runnable code) {
		this.code = code;
	}
	
}
